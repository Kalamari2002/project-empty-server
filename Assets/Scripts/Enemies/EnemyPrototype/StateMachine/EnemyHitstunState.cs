using UnityEngine;

public class EnemyHitstunState : EnemyBaseState
{
    int _animationIndex;
    float _animationDuration;
    
    public EnemyHitstunState(EnemyStateMachine context, EnemyStateFactory factory, int hitStunAnimationIndex)
    : base(context, factory)
    {
        name = "Hitstun";
        isRootState = true;
        _animationIndex = hitStunAnimationIndex;
    }
    public override void EnterState()
    {
        _context.Animator.speed = 0.5f;
        _context.Animator.Play("EnemyPrototypeHitstun" + _animationIndex.ToString(), -1, 0);
        _animationDuration = _context.Animator.GetCurrentAnimatorClipInfo(0).Length;
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
        _context.Animator.speed = 1.0f;
    }
}
