/**
* If you're surprised by the sheer amount of nothing happening
* in this state, it's cause the actual crouching is handled by
* the animator.
*/
public class PlayerCrouchState : PlayerBaseState
{
    public PlayerCrouchState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory){}
    public override void EnterState()
    {
        name = "Crouch";
        _context.CurrentDrag = _context.CrouchDrag;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState(){}
    public override void ExitState(){}
    public override void CheckSwitchStates()
    {
        if (!_context.IsCrouchPressed)
        {
            SwitchState(_factory.Run());
        }
    }
    public override void InitializeSubState(){}
}
