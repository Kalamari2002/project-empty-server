public class PlayerStateFactory
{
    PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }
    public PlayerBaseState Grounded()
    {
        return new PlayerGroundedState(_context, this);
    }
    public PlayerBaseState Move()
    {
        return new PlayerMoveState(_context, this);
    }
    public PlayerBaseState Run()
    {
        return new PlayerRunState(_context, this);
    }
    public PlayerCrouchState Crouch()
    {
        return new PlayerCrouchState(_context, this);
    }

    public PlayerAirborneState Airborne()
    {
        return new PlayerAirborneState(_context, this);
    }
    public PlayerFreefallState Freefall()
    {
        return new PlayerFreefallState(_context, this);
    }
}
