using UnityEngine;

public class HandPickupIdleState : HandBaseState
{
    public HandPickupIdleState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "PickupIdle";
        InitializeSubState();
    }

    public override void EnterState()
    {
        _context.CanPunch = true;
        _context.Animator.speed = 1.0f;
        _context.Animator.Play("HandPickupIdle", -1, 0);
        Debug.Log("ENTERED IDLE STATE");
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
    }
    public override void ExitState()
    {

    }
    public override void CheckSwitchStates()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SwitchState(_factory.PickupPrimaryAction());
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (!_context.Grounded && _context.CanDropKick && _context.KickChargeTime >= _context.MaxKickChargeTime && !_context.WallRunning)
            {
                SwitchState(_factory.DropKick());
            }
            else if (_context.Grounded || !_context.WallRunning)
            {
                SwitchState(_factory.Kick(_context.KickChargeTime));
            }
        }
    }
    public override void InitializeSubState()
    {
    }
}
