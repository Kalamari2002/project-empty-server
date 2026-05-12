
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
        _currentSubState?.UpdateStates();
    }
    public void FixedUpdateStates()
    {
        FixedUpdateState();
        _currentSubState?.FixedUpdateStates();
    }
    protected void SwitchState(PlayerBaseState newState)
    {
        ExitState();

        newState.EnterState();

        /**
        * Prevents current state from becoming a substate. For example,
        * we only ever want to set the current state to Grounded, but never
        * Move or Slide. This way the transition to Airborne stays consistent
        * no matter which substate you're in.
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