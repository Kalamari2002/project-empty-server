using UnityEngine;

public class HandGrabState : HandBaseState
{
    int _currentPunch = 1;
    bool _startSwitchCountDown = false;
    bool _switchStateOnNextAttack = false;
    float _animationDuration;

    public HandGrabState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "Grab";
        _context.CanPunch = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Hand entered Grab state");
        _context.Animator.speed = 1.0f;
        _context.Animator.Play("HandGrabIdle", -1, 0);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        if (_context.CanPunch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                switch (_currentPunch)
                {
                    case 3:
                        _context.Animator.Play("HandGrabPunchFinal", -1, 0);
                        _animationDuration = _context.Animator.GetCurrentAnimatorClipInfo(0).Length;
                        _startSwitchCountDown = true;
                        _switchStateOnNextAttack = true;
                        break;

                    default:
                        _context.Animator.Play("HandGrabPunch", -1, 0);
                        _currentPunch++;
                        break;
                }
                _context.CanPunch = false;
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _context.Animator.Play("HandGrabKick", -1, 0);
                _animationDuration = _context.Animator.GetCurrentAnimatorClipInfo(0).Length;
                _startSwitchCountDown = true;
                _switchStateOnNextAttack = true;
                _context.CanPunch = false;
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
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {
        if (_startSwitchCountDown && _animationDuration <= 0)
        {
            SwitchState(_factory.Move());
        }
        else if (_context.CanPunch)
        {
            if (_switchStateOnNextAttack)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    SwitchState(_factory.PunchOne());
                }
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    SwitchState(_factory.Kick());
                }
                if (Input.GetMouseButtonDown(1) && _context.CastGrabHit())
                {
                    SwitchState(_factory.Grab());
                }
            }
            else if (!Input.GetMouseButton(1))
            {
                _context.ReleaseGrab();
                SwitchState(_factory.Move());
            }
        }
    }
    public override void InitializeSubState()
    {

    }
}
