using UnityEngine;

public class PlayerAirborneState : PlayerBaseState
{
    public PlayerAirborneState(PlayerStateMachine context, PlayerStateFactory factory)
    : base(context, factory)
    {
        _isRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        _context.CurrentDrag = 0;
        _context.OnEnterState("Airborne");
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
        _context.PlayerRigidBody.linearDamping = 0;
        LimitSpeed();
        Move();
    }
    public override void ExitState()
    {
        _currentSubState?.ExitState();
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
        SetSubState(_factory.Freefall());
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
    void Move()
    {
        float horizontal = _context.HorizontalInput;
        float vertical = _context.VerticalInput;
        float multiplier = _context.AirMultiplier;
        Transform orientation = _context.PlayerOrientation;
        Vector3 directionVector = orientation.forward * vertical + orientation.right * horizontal;
        Rigidbody rb = _context.PlayerRigidBody;

        rb.AddForce(directionVector.normalized * _context.AirSpeed * multiplier);
    }
}
