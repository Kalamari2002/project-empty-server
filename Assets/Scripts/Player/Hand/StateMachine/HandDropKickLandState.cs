using UnityEngine;

public class HandDropKickLandState : HandBaseState
{
    float _animationDuration;

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
        _context.DropKicking = false;
    }
    public override void CheckSwitchStates()
    {
        if (_animationDuration <= 0)
        {
            SwitchState(_factory.Move());
        }
    }
    public override void InitializeSubState()
    {

    }
}
