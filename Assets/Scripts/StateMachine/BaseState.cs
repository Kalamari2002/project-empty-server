public abstract class BaseState
{
    protected BaseStateMachine stateMachine;
    protected BaseStateFactory stateFactory;

    protected BaseState currentSuperState;
    protected BaseState currentSubState;

    public BaseState(BaseStateMachine stateMachine, BaseStateFactory stateFactory)
    {
        this.stateMachine = stateMachine;
        this.stateFactory = stateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();
    void UpdateStates() { }
    protected void SwitchStates(BaseState newState) 
    {
        ExitState();
        newState.EnterState();
        stateMachine.CurrentState = newState;
    }
    protected void SetSuperState(BaseState newSuperState) 
    {
        currentSuperState = newSuperState;
    }
    protected void SetSubState(BaseState newSubState) 
    {
        currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
