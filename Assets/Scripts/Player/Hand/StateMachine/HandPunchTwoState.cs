using UnityEngine;

public class HandPunchTwoState : HandBaseState
{
    float _animationDuration;

    public HandPunchTwoState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "PunchTwo";
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Hand entered Punch Two state");
        _context.CanPunch = false;
        _context.Animator.speed = 1.0f;
        _context.Animator.Play("HandPunch2", -1, 0);
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
                SwitchState(_factory.PunchThree());
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
