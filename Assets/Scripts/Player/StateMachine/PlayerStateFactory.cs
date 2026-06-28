public class PlayerStateFactory : BaseStateFactory
{
    PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext) : base(currentContext)
    {
        _context = currentContext;
    }
    public BaseState Grounded()
    {
        return new PlayerGroundedState(_context, this);
    }
    public BaseState Move()
    {
        return new PlayerMoveState(_context, this);
    }
    public BaseState Slide()
    {
        return new PlayerSlideState(_context, this);   
    }
    public BaseState Run()
    {
        return new PlayerRunState(_context, this);
    }
    public BaseState Crouch()
    {
        return new PlayerCrouchState(_context, this);
    }

    public BaseState Airborne()
    {
        return new PlayerAirborneState(_context, this);
    }
    public BaseState AirMove()
    {
        return new PlayerAirMoveState(_context, this);
    }
    public BaseState Freefall()
    {
        return new PlayerFreefallState(_context, this);
    }
    public BaseState WallRun()
    {
        return new PlayerWallRunState(_context, this);
    }
    public BaseState AirCrouch()
    {
        return new PlayerAirCrouchState(_context, this);
    }
}
