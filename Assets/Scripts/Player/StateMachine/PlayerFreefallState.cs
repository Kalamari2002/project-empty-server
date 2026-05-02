using UnityEngine;

public class PlayerFreefallState : PlayerBaseState
{

    public PlayerFreefallState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory){}

    public override void EnterState(){}
    public override void UpdateState(){}
    public override void FixedUpdateState()
    {
        Move();
    }
    public override void ExitState(){}
    public override void CheckSwitchStates(){}
    public override void InitializeSubState(){}

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
