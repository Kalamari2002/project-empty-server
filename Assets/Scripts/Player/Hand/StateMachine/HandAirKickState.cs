using UnityEngine;

public class HandAirKickState : HandBaseState
{
    float _animationDuration;

    public HandAirKickState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "AirKick";
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Hand entered Air Kick state");
        _context.CanPunch = false;
        _context.Animator.speed = 1;
        _context.Animator.Play("Kick", -1, 0);
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
        if (_context.CanPunch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SwitchState(_factory.AirPunch());
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) && _context.IsTouchingWall() == 0)
            {
                if (_context.KickChargeTime < _context.MaxKickChargeTime)
                {
                    SwitchState(_factory.AirKick());
                }
                else
                {
                    SwitchState(_factory.DropKick());
                }
            }
        }

        if (_animationDuration <= 0)
        {
            SwitchState(_factory.AirMove());
        }
    }
    public override void InitializeSubState()
    {

    }
}
