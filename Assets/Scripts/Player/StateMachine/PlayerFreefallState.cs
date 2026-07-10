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
        Crouch();
    }
    public override void InitializeSubState(){}
    void Crouch()
    {

        if (_context.PressedCrouch)
        {
            SwitchState(_factory.AirCrouch());
        }
    }
}
