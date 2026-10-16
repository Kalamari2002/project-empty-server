using UnityEngine;

public class HandDropKickState : HandBaseState
{
    public HandDropKickState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "DropKick";
        name = "DropKick";
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Hand entered Drop Kick state");
        _context.CanPunch = false;
        _context.DropKicking = true;
        // this should only be set back to true on HandDropKickLandState.ExitState
        _context.CanDropWeapon = false;
        _context.Animator.speed = 1;
        _context.LastKickCharge = _context.MaxKickChargeTime;
        _context.Animator.Play("DropKick", -1, 0);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
    }
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {
        if (_context.CurrentWeapon != null && _context.Grounded)
        {
            SwitchState(_factory.DropKickLand());
        } 
    }
    public override void InitializeSubState()
    {

    }
}
