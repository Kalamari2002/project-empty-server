using UnityEngine;
public abstract class BaseState
{
    protected BaseStateMachine stateMachine;
    protected BaseStateFactory stateFactory;

    protected BaseState currentSuperState;
    protected BaseState currentSubState;

    protected bool isRootState = false;

    protected string name;

    public BaseState SubState { get { return currentSubState; } }

    public BaseState(BaseStateMachine stateMachine, BaseStateFactory stateFactory)
    {
        this.stateMachine = stateMachine;
        this.stateFactory = stateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();
    public void UpdateStates() 
    {
        UpdateState();
        if (currentSubState != null)
        {
            currentSubState.UpdateStates();
        }
    }

    public void FixedUpdateStates()
    {
        FixedUpdateState();
        if (currentSubState != null)
        {
            currentSubState.FixedUpdateStates();
        }
    }

    protected void SwitchState(BaseState newState) 
    {
        ExitState();
        newState.EnterState();
        if (isRootState)
        {
            Debug.Log("Root State switched to: " + newState);
            stateMachine.CurrentState = newState;
        }
        else if (currentSuperState != null)
        {
            Debug.Log("Sub State switched to: " + newState);
            currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(BaseState newSuperState) 
    {
        Debug.Log(this + " setting new Super State: " + newSuperState);
        currentSuperState = newSuperState;
    }

    protected void SetSubState(BaseState newSubState) 
    {
        Debug.Log(this + " setting new Sub State: " + newSubState);
        currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

    public override string ToString()
    {
        return name;
    }
}
