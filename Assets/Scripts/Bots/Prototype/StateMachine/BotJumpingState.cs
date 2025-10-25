public class BotJumpingState : BotBaseState
{
    public BotJumpingState(BotStateMachine stateMachine, BotStateFactory stateFactory) : base(stateMachine, stateFactory){}

    public override void CheckSwitchStates()
    {
        throw new System.NotImplementedException();
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
