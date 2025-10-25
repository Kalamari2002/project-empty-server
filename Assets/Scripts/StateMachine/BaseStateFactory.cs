public class BaseStateFactory
{
    protected BaseStateMachine stateMachine;
    public BaseStateFactory (BaseStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
