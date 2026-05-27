public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine context, PlayerStateFactory factory)
    : base(context, factory){}
    
    public override void EnterState(){}
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState(){}
    public override void ExitState(){}
    public override void CheckSwitchStates()
    {
        if (_context.IsDirectionPressed)
        {
            SwitchState(_factory.Move());
        }
    }
    public override void InitializeSubState(){}
}
