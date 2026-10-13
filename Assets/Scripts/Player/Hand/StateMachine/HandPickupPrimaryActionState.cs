using UnityEngine;

public class HandPickupPrimaryActionState : HandBaseState
{
    public HandPickupPrimaryActionState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "PrimaryAction";
        InitializeSubState();
    }

    public override void EnterState()
    {
        _context.CanPunch = false;
        _context.CanDropWeapon = false;
        _context.CurrentWeapon.PrimaryAction.ExecutingAction = true;
        Debug.Log("Primary Action Entered!");
    }
    public override void UpdateState()
    {
        _context.CurrentWeapon.PrimaryAction.Action(_context);
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
    }
    public override void ExitState()
    {
        _context.CanPunch = true;
        _context.CanDropWeapon = true;
        _context.CurrentWeapon.PrimaryAction.ExecutingAction = false;
        _context.CurrentWeapon.PrimaryAction.ResetAction();
    }
    public override void CheckSwitchStates()
    {
        if (_context.CurrentWeapon.PrimaryAction.EndAction)
        {
            SwitchState(_factory.PickupIdle());
        }
        else if (_context.CurrentWeapon.PrimaryAction.CanInterruptAction && Input.GetMouseButtonDown(0))
        {
            SwitchState(_factory.PickupPrimaryAction());
        }
    }
    public override void InitializeSubState()
    {
    }
}
