using UnityEngine;

public class HandDropKickLandState : HandBaseState
{
    float _animationDuration = 0.466f;

    public HandDropKickLandState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "DropKickLand";
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Hand entered Drop Kick Land state");
        _context.CanPunch = false;
        _context.Animator.speed = 1;
        _context.Animator.Play("DropKickLand", -1, 0);
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
        // this is initially set to false on HandDropKickState.EnterState
        _context.CanDropWeapon = true;
        _context.DropKicking = false;
    }
    public override void CheckSwitchStates()
    {
        if (_animationDuration <= 0)
        {
            SwitchState(_context.CurrentWeapon == null ? _factory.Move() : _factory.PickupIdle());
        }
    }
    public override void InitializeSubState()
    {

    }
}
