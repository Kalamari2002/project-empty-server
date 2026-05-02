
using Unity.VisualScripting;

/**
* ABSTRACT STATE
* Blueprint for concrete states
*/
public abstract class PlayerBaseState
{
    protected bool _isRootState = false;
    protected PlayerStateMachine _context;
    protected PlayerStateFactory _factory;
    protected PlayerBaseState _currentSuperState, _currentSubState;
    
    public PlayerBaseState(PlayerStateMachine context, PlayerStateFactory factory)
    {
        _context = context;
        _factory = factory;
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
        _currentSubState?.UpdateState();
    }
    public void FixedUpdateStates()
    {
        FixedUpdateState();
        _currentSubState?.FixedUpdateState();
    }
    protected void SwitchState(PlayerBaseState newState)
    {
        ExitState();

        newState.EnterState();

        /**
        * Prevents current state from becoming a substate. For example,
        * we only ever want to set the current state to Grounded, but never
        * Move or Idle. Otherwise, you'd need to implement the jump logic
        * for each substate individually.  
        */
        if (_isRootState)
        {
            _context.CurrentState = newState;   
        }else if(_currentSuperState != null)
        {
            /**
            * If this is a substate, then we want its superstate's  
            * substate to change from this one to the new one. Pretty
            * much transfering states.
            */
            _currentSuperState.SetSubState(newState);
        }
    }
    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}