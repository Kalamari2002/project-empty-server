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

    public BaseState Grounded()
    {
        return new BotGroundedState(botStateMachine, this);
    }

    public BaseState Jump()
    {
        return new BotJumpingState(botStateMachine, this);
    }

    public BaseState Chasing()
    {
        return new BotChasingState(botStateMachine, this);
    }
}
