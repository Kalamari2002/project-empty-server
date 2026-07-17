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

        /**
        * Prevents current state from becoming a substate. For example,
        * we only ever want to set the current state to Grounded, but never
        * Move or Slide. This way the transition to Airborne stays consistent
        * no matter which substate you're in.
        */
        if (isRootState)
        {
            Debug.Log("Root State switched to: " + newState.ToString());
            stateMachine.CurrentState = newState;
        }
        else if (currentSuperState != null)
        {
            /**
            * If this is a substate, then we want its superstate's  
            * substate to change from this one to the new one. Pretty
            * much transfering states.
            */
            Debug.Log("Sub State switched to: " + newState.ToString());
            currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(BaseState newSuperState) 
    {
        Debug.Log(ToString() + " setting new Super State: " + newSuperState.ToString());
        currentSuperState = newSuperState;
    }

    protected void SetSubState(BaseState newSubState) 
    {
        Debug.Log(ToString() + " setting new Sub State: " + newSubState.ToString());
        currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

    public BaseState GetDeepestActiveState()
    {
        if (!isRootState)
            return null;
        return ActiveStateHelper(this);
    }

    BaseState ActiveStateHelper(BaseState currentState)
    {
        if (currentState.currentSubState == null)
            return currentState;
        return ActiveStateHelper(currentState.currentSubState);
    }

    public override string ToString()
    {
        return name;
    }
}
