using UnityEngine;
using UnityEngine.InputSystem;

public class HandAirMoveState : HandBaseState
{
    public HandAirMoveState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "AirMove";
        InitializeSubState();
    }

    public override void EnterState() 
    {
        _context.CanPunch = true;
        _context.Animator.speed = 1.0f;
        _context.Animator.Play("HandIdle", -1, 0);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState() { }
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {
        if (_context.CanPunch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SwitchState(_factory.AirPunch());
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) && _context.IsTouchingWall() == 0)
            {
                if (_context.CanDropKick && _context.KickChargeTime >= _context.MaxKickChargeTime)
                {
                    SwitchState(_factory.DropKick());
                }
                else
                {
                    SwitchState(_factory.AirKick(_context.KickChargeTime));
                }
            }
        }
        
    }
    public override void InitializeSubState()
    {
        //if (_context.IsCrouchPressed)
        //    SetSubState(_factory.AirCrouch());
        //else
        //    SetSubState(_factory.Freefall());
        //currentSubState.EnterState();
    }
}
