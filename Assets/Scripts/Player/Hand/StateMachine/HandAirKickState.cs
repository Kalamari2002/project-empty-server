using UnityEngine;

public class HandAirKickState : HandBaseState
{
    float _animationDuration;
    float _kickCharge;

    public HandAirKickState(HandStateMachine context, HandStateFactory factory, float kickCharge)
    : base(context, factory)
    {
        StateName = "AirKick";
        _kickCharge = kickCharge;
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Hand entered Air Kick state");
        _context.CanPunch = false;
        _context.Animator.speed = 1;
        _context.Animator.Play("Kick", -1, 0);
        _context.LastKickCharge = _kickCharge;
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
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                SwitchState(_factory.AirKick(_context.KickChargeTime));
            }
        }
    }
    public override void InitializeSubState()
    {

    }
}
