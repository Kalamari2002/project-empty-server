using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory factory)
    : base(context, factory)
    {
        InitializeSubState();
        _isRootState = true;
    }

    public override void EnterState(){
        Jump();
    }
    public override void UpdateState(){}
    public override void FixedUpdateState(){}
    public override void ExitState(){}
    public override void CheckSwitchStates(){}
    public override void InitializeSubState(){}

    void Jump()
    {
        Rigidbody rb = _context.PlayerRigidBody;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * _context.JumpForce, ForceMode.Impulse);
    }
}
