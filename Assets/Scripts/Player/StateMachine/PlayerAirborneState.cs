using UnityEngine;

public class PlayerAirborneState : PlayerBaseState
{
    const float MIN_VELOCITY_TO_ROLL = -6f;
    const float FALL_DAMAGE_THRESHOLD = -9f;
    const float FALL_DAMAGE_MULTIPLIER = 1.0f;
    const float ROLL_WINDOW = 0.4f;
    const float ROLL_ATTEMPT_RESET_TIME = 0.7f;
    
    float timeSinceCrouchPressed;
    GameObject _lastWallRunSurface; // god i fucking hate the name of this variable.
    public GameObject LastWallRunSurface { get { return _lastWallRunSurface; } set { _lastWallRunSurface = value; } }

    public PlayerAirborneState(PlayerStateMachine context, PlayerStateFactory factory)
    : base(context, factory)
    {
        isRootState = true;
        name = "Airborne";
        _lastWallRunSurface = null;
        timeSinceCrouchPressed = -1;
        InitializeSubState();
    }

    public override void EnterState()
    {
        _context.CurrentDrag = 0;
        _context.OrientationAnimator.SetBool("Grounded", false);
    }
    public override void UpdateState()
    {
        CheckRollCooldown();
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
        _context.PlayerRigidBody.linearDamping = 0;
        LimitSpeed();
    }
    public override void ExitState()
    {
        currentSubState?.ExitState();
    }
    public override void CheckSwitchStates()
    {
        if (_context.Grounded)
        {
            OnLanding();
            SwitchState(_factory.Grounded());
        }
    }
    public override void InitializeSubState()
    {
        SetSubState(_factory.AirMove());
    }
    void LimitSpeed()
    {
        Rigidbody rb = _context.PlayerRigidBody;
        float speedLimit = _context.MaxAirSpeed;
        Vector3 xzVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (xzVelocity.magnitude > speedLimit)
        {
            xzVelocity = xzVelocity.normalized * speedLimit;
            rb.linearVelocity = new Vector3(xzVelocity.x, rb.linearVelocity.y, xzVelocity.z);
        }
    }
    void CheckRollCooldown()
    {
        if(_context.PressedCrouch && timeSinceCrouchPressed < 0)
            timeSinceCrouchPressed = 0;

        if(timeSinceCrouchPressed >= 0)
            timeSinceCrouchPressed += Time.deltaTime;
        
        if(timeSinceCrouchPressed >= ROLL_ATTEMPT_RESET_TIME)
            timeSinceCrouchPressed = -1;
    }
    void OnLanding()
    {
        float linearVelocityY = _context.PlayerRigidBody.linearVelocity.y;
        bool succeededRoll = 
            _context.IsCrouchPressed 
            && linearVelocityY < MIN_VELOCITY_TO_ROLL 
            && timeSinceCrouchPressed > 0 
            && timeSinceCrouchPressed <= ROLL_WINDOW;

        if(succeededRoll)
        {
            _context.RollOnGrounded = true;
            return;
        }
        if (linearVelocityY < FALL_DAMAGE_THRESHOLD)
            FallDamage();
    }
    void FallDamage()
    {
        int damage = Mathf.RoundToInt(Mathf.Abs(_context.PlayerRigidBody.linearVelocity.y) * FALL_DAMAGE_MULTIPLIER);
        _context.TakeDamage(damage);
    }

}
