using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateMachine context, EnemyStateFactory factory)
    : base(context, factory)
    {
        name = "Chase";
        isRootState = true;
        //InitializeSubState();
    }
    public override void EnterState()
    {
        _context.Animator.speed = 1;
        _context.Animator.Play("EnemyPrototypeRun", -1, 0);
    }
    public override void InitializeSubState()
    {

    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        _context.MoveTowards(_context.Player, _context.transform.forward, _context.ChaseSpeed);
    }

    public override void FixedUpdateState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (Vector3.Distance(_context.transform.position, _context.Player.position) <= _context.StopDistance)
        {
            SwitchState(_factory.Idle());
        }
    }

    public override void ExitState()
    {

    }
}
