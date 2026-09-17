using UnityEngine;

public class EnemyPunchState : EnemyBaseState
{
    float _animationDuration;

    public EnemyPunchState(EnemyStateMachine context, EnemyStateFactory factory)
    : base(context, factory)
    {
        name = "Punch";
        isRootState = true;
    }

    public override void EnterState()
    {
        _context.Animator.speed = 0.5f;
        _context.Animator.Play("EnemyPrototypePunch", -1, 0);
        _animationDuration = _context.Animator.GetCurrentAnimatorClipInfo(0).Length / _context.Animator.speed;
    }

    public override void InitializeSubState()
    {

    }

    public override void UpdateState()
    {
        _animationDuration -= Time.deltaTime;
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (_animationDuration <= 0)
        {
            SwitchState(_factory.Idle());
        }
    }

    public override void ExitState()
    {
        _context.Animator.speed = 1f;
    }
}
