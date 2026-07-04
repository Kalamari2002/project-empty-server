using UnityEngine;

public class PlayerAirborneState : PlayerBaseState
{

    GameObject _lastWallRunSurface; // god i fucking hate the name of this variable.
    public GameObject LastWallRunSurface { get { return _lastWallRunSurface; } set { _lastWallRunSurface = value; } }

    public PlayerAirborneState(PlayerStateMachine context, PlayerStateFactory factory)
    : base(context, factory)
    {
        isRootState = true;
        StateName = "Airborne";
        _lastWallRunSurface = null;
        InitializeSubState();
    }

    public override void EnterState()
    {
        _context.CurrentDrag = 0;
    }
    public override void UpdateState()
    {
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

}
