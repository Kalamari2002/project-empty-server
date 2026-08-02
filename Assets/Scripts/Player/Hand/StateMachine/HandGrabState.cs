using UnityEngine;

public class HandGrabState : HandBaseState
{
    bool _startSwitchCountDown = false;
    bool _switchStateOnNextAttack = false;
    float _animationDuration;

    public HandGrabState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "Grab";
    }

    /**
     * Level 5 btw
     * https://www.youtube.com/watch?v=FeWVA2tp9YI
     */
    public override void EnterState()
    {
        if (!_context.Grabbing)
        {
            _context.Animator.speed = 1.0f;
            _context.Animator.Play("HandGrabIdle", -1, 0);
            _context.CanPunch = true;
        }
        _context.Grabbing = true;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        if (_context.CanPunch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                switch (_context.GrabPunchAnimation)
                {
                    case 3:
                        _context.Animator.Play("HandGrabPunchFinal", -1, 0);
                        _animationDuration = _context.Animator.GetCurrentAnimatorClipInfo(0).Length;
                        _startSwitchCountDown = true;
                        _switchStateOnNextAttack = true;
                        _context.GrabPunchAnimation = 1;
                        break;

                    default:
                        _context.Animator.Play("HandGrabPunch", -1, 0);
                        _context.GrabPunchAnimation++;
                        break;
                }
                _context.CanPunch = false;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) && _context.IsTouchingWall() == 0 && _context.CanPunch)
            {
                if (_context.KickChargeTime < _context.MaxKickChargeTime || _context.Grounded)
                {
                    _context.Animator.Play("HandGrabKick", -1, 0);
                    _animationDuration = _context.Animator.GetCurrentAnimatorClipInfo(0).Length;
                    _startSwitchCountDown = true;
                    _switchStateOnNextAttack = true;
                    _context.CanPunch = false;
                }
            }
        }
        if (_startSwitchCountDown)
        {
            _animationDuration -= Time.deltaTime;
        }
    }
    public override void FixedUpdateState()
    {
    }
    
    public override void ExitState() 
    {

    }

    public override void CheckSwitchStates()
    {
        if (_startSwitchCountDown && _animationDuration <= 0)
        {
            SwitchStateWrapper(_context.Grounded ? _factory.Move() : _factory.AirMove());
        }
        else if (_context.CanPunch)
        {
            if (_switchStateOnNextAttack)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    SwitchStateWrapper(_context.Grounded ? _factory.PunchOne() : _factory.AirPunch());
                }
                if (Input.GetKeyUp(KeyCode.LeftShift) && _context.IsTouchingWall() == 0 && _context.KickChargeTime < _context.MaxKickChargeTime)
                {
                    SwitchStateWrapper(_context.Grounded ? _factory.Kick() : _factory.AirKick());
                }
                if (Input.GetMouseButtonDown(1) && !_context.Grabbing && _context.CastGrabHit())
                {
                    SwitchStateWrapper(_factory.Grab());
                }
            }
            else if (!Input.GetMouseButton(1))
            {
                _context.ReleaseGrab();
                SwitchStateWrapper(_context.Grounded ? _factory.Move() : _factory.AirMove());
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) && _context.IsTouchingWall() == 0 && _context.KickChargeTime >= _context.MaxKickChargeTime)
            {
                _context.ReleaseGrab();
                SwitchStateWrapper(_factory.DropKick());
            }
        }
    }

    public override void InitializeSubState()
    {

    }

    void SwitchStateWrapper(BaseState newState)
    {
        _context.Grabbing = false;
        _context.GrabPunchAnimation = 1;
        SwitchState(newState);
    }
}
