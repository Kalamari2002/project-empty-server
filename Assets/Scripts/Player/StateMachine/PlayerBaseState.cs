
/**
* ABSTRACT STATE
* Blueprint for concrete states
*/
public abstract class PlayerBaseState : BaseState
{
    protected PlayerStateMachine _context;
    protected PlayerStateFactory _factory;
    
    public PlayerBaseState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
        _context = context;
        _factory = factory;
        name = "";
    }
}