using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine context, EnemyStateFactory factory)
    : base(context, factory)
    {
        name = "Idle";
        isRootState = true;
    }
    public override void EnterState()
    {
        _context.Animator.speed = 0.5f;
        _context.Animator.Play("EnemyPrototypeIdle", -1, 0);
    }

    public override void InitializeSubState()
    {

    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {

    }

    public override void CheckSwitchStates()
    {
        float distanceFromPlayer = Vector3.Distance(_context.transform.position, _context.Player.position);
        if (distanceFromPlayer <= _context.MinimumChaseDistance)
        {
            if (_context.StopDistance < distanceFromPlayer)
            {
                SwitchState(_factory.Chase());
            }
            else if (_context.CanAtack)
            {
                SwitchState(_factory.Punch());
            }
        }
    }

    public override void ExitState()
    {
        _context.Animator.speed = 1.0f;
    }
}
