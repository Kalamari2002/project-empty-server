using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory){}
    public override void EnterState()
    {
        Debug.Log("Run");
        _context.CurrentDrag = _context.Drag;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState(){}
    public override void ExitState(){}
    public override void CheckSwitchStates()
    {
        if (_context.IsCrouchPressed)
        {
            SwitchState(_factory.Crouch());
        }
    }
    public override void InitializeSubState(){}
}
