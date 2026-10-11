using UnityEngine;

public class HandPickupPrimaryActionState : HandBaseState
{
    public HandPickupPrimaryActionState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "Primary Action";
        InitializeSubState();
    }

    public override void EnterState()
    {
        _context.CurrentWeapon.PrimaryAction.ActionDuration = _context.CurrentWeapon.PrimaryAction.ActionInitialDuration;
        _context.CurrentWeapon.PrimaryAction.ExecutingAction = true;
        _context.CurrentWeapon.PrimaryAction.Action(_context);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        _context.CurrentWeapon.PrimaryAction.ActionDuration -= Time.deltaTime;
    }
    public override void FixedUpdateState()
    {
    }
    public override void ExitState()
    {
        _context.CurrentWeapon.PrimaryAction.ActionDuration = _context.CurrentWeapon.PrimaryAction.ActionInitialDuration;
        _context.CurrentWeapon.PrimaryAction.ExecutingAction = false;
    }
    public override void CheckSwitchStates()
    {
        if (_context.CurrentWeapon.PrimaryAction.ActionDuration <= 0)
        {
            SwitchState(_factory.PickupIdle());
        }
    }
    public override void InitializeSubState()
    {
    }
}
