using UnityEngine;

public class HandPunchOneState : HandBaseState
{
    float _animationDuration;

    public HandPunchOneState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "PunchOne";
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Hand entered Punch One state");
        _context.CanPunch = false;
        _context.Animator.speed = 1.5f;
        _context.Animator.Play("HandPunch1", -1, 0);
        _animationDuration = _context.Animator.GetCurrentAnimatorClipInfo(0).Length / 1.5f;
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
                SwitchState(_factory.PunchTwo());
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                SwitchState(_factory.Kick());
            }
        }

        if (_animationDuration <= 0)
        {
            SwitchState(_factory.Move());
        }
    }
    public override void InitializeSubState()
    {

    }
}
