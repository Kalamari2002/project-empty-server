
using Unity.VisualScripting;

/**
* ABSTRACT STATE
* Blueprint for concrete states
*/
public abstract class PlayerBaseState : BaseState
{
    protected PlayerStateMachine _context;
    protected PlayerStateFactory _factory;
    string _stateName;
    public string StateName { get { return _stateName; } set { _stateName = value; } }
    public PlayerBaseState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
        _context = context;
        _factory = factory;
        _stateName = "";
    }
}