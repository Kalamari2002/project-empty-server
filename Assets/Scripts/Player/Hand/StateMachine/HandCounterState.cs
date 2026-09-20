using UnityEngine;

public class HandCounterState : HandBaseState
{
    float _animationDuration;

    public HandCounterState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "Counter";
        InitializeSubState();
    }

    public override void EnterState()
    {
        _context.CanPunch = false;
        _context.Animator.speed = 1.0f;
        _context.Animator.Play("HandCounter", -1, 0);
        _animationDuration = _context.Animator.GetCurrentAnimatorClipInfo(0).Length;
    }
    public override void UpdateState()
    {
        _animationDuration -= Time.deltaTime;
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
    }
    public override void ExitState()
    {

    }
    public override void CheckSwitchStates()
    {
        if (_animationDuration <= 0)
        {
            SwitchState(_factory.Move());
        }
        else if (_context.CanPunch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SwitchState(_factory.PunchOne());
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                SwitchState(_factory.Kick(_context.KickChargeTime));
            }
            else if (Input.GetMouseButtonDown(1))
            {
                switch(_context.CastGrabHit())
                {
                    case GrabActionsEnums.COUNTER:
                        SwitchState(_factory.Counter());
                        break;
                    case GrabActionsEnums.GRAB:
                        SwitchState(_factory.Grab());
                        break;

                }
            }
        }
    }
    public override void InitializeSubState()
    {
        //if (_context.IsCrouchPressed)
        //    SetSubState(_factory.Crouch());
        //else
        //    SetSubState(_factory.Run());
        //currentSubState.EnterState();
    }
}
