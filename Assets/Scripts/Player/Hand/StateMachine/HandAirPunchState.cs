using UnityEngine;

public class HandAirPunchState : HandBaseState
{
    float _animationDuration;

    public HandAirPunchState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "AirPunch";
        InitializeSubState();
    }

    public override void EnterState()
    {
        _context.CanPunch = false;
        _context.Animator.speed = 1.0f;
        _context.Animator.Play("HandAirPunch", -1, 0);
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
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {
        if (_animationDuration <= 0)
        {
            SwitchState(_factory.AirMove());
        }
        else if (_context.CanPunch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SwitchState(_factory.AirPunch());
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) && _context.IsTouchingWall() == 0)
            {
                if (_context.CanDropKick && _context.KickChargeTime >= _context.MaxKickChargeTime)
                {
                    SwitchState(_factory.DropKick());
                }
                else
                {
                    SwitchState(_factory.AirKick(_context.KickChargeTime));
                }
            }
        }
    }
    public override void InitializeSubState()
    {

    }
}
