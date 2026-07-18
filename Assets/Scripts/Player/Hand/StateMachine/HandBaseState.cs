public abstract class HandBaseState : BaseState
{
    protected HandStateMachine _context;
    protected HandStateFactory _factory;
    string _stateName;
    public string StateName { get { return _stateName; } set { _stateName = value; } }
    public HandBaseState(HandStateMachine context, HandStateFactory factory) : base(context, factory)
    {
        _context = context;
        _factory = factory;
        _stateName = "";
    }
}
