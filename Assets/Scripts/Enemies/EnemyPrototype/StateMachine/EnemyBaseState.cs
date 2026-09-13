public abstract class EnemyBaseState : BaseState
{
    protected EnemyStateMachine _context;
    protected EnemyStateFactory _factory;

    public EnemyBaseState(EnemyStateMachine context, EnemyStateFactory factory) : base(context, factory)
    {
        _context = context;
        _factory = factory;
        name = "";
    }
}
