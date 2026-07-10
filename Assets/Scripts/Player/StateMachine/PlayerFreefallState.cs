using System;
using UnityEngine;

public class PlayerFreefallState : PlayerBaseState
{
    public PlayerFreefallState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory)
    {
        name = "Freefall";
    }

    public override void EnterState(){}
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState(){}
    public override void ExitState(){}
    public override void CheckSwitchStates()
    {
        if (_context.PressedCrouch && _context.OrientationAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            SwitchState(_factory.AirCrouch());
        }
    }
    public override void InitializeSubState(){}
}
