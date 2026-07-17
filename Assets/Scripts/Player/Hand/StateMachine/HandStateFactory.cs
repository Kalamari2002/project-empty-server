public class HandStateFactory : BaseStateFactory
{
    HandStateMachine _context;

    public HandStateFactory(HandStateMachine currentContext) : base(currentContext)
    {
        _context = currentContext;
    }

    public BaseState Move()
    {
        return new HandMoveState(_context, this);
    }

    public BaseState Grounded()
    {
        return new HandGroundedState(_context, this);
    }

    public BaseState PunchOne()
    {
        return new HandPunchOneState(_context, this);
    }

    public BaseState PunchTwo()
    {
        return new HandPunchTwoState(_context, this);
    }

    public BaseState PunchThree()
    {
        return new HandPunchThreeState(_context, this);
    }

    public BaseState Kick()
    {
        return new HandKickState(_context, this);
    }

    public BaseState Grab()
    {
        return new HandGrabState(_context, this);
    }

    public BaseState Airborne()
    {
        return new HandAirborneState(_context, this);
    }

    public BaseState AirMove()
    {
        return new HandAirMoveState(_context, this);
    }
}
