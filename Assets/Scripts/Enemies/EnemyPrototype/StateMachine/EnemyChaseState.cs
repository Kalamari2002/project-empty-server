using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    Transform target;
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
        target = _context.CurrentWeapon == null ? _context.PickTarget() : _context.Player;
        CheckSwitchStates();
        _context.MoveTowards(target, _context.transform.forward, _context.ChaseSpeed);
    }

    public override void FixedUpdateState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (Vector3.Distance(_context.transform.position, target.position) <= _context.StopDistance)
        {
            WeaponProp weaponProp = target.GetComponent<WeaponProp>();
            if (_context.CurrentWeapon == null && weaponProp != null)
            {
                _context.PickUpWeapon(weaponProp);
            }
            else
            {
                SwitchState(_factory.Idle());
            }
        }
    }

    public override void ExitState()
    {

    }
}
