public class BotStateFactory : BaseStateFactory
{
    BotStateMachine botStateMachine;
    public BotStateFactory(BotStateMachine stateMachine) : base(stateMachine) 
    { 
        botStateMachine = stateMachine;
    }

    public BaseState Patrol()
    {
        return new BotPatrolingState(botStateMachine, this);
    }

    public BaseState Engage()
    {
        return new BotEngagedState(botStateMachine, this);
    }
}
