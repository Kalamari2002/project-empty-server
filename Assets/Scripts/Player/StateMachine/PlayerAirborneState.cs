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
        Debug.Log("Airborne");
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
    public override void ExitState(){}
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
}
