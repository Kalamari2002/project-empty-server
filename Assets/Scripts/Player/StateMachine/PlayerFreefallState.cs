using System;
using UnityEngine;

public class PlayerFreefallState : PlayerBaseState
{
    float _crouchTimer;
    public PlayerFreefallState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory)
    {
        name = "Freefall";
        _crouchTimer = -1.0f;
    }

    public override void EnterState(){}
    public override void UpdateState()
    {
        if(_crouchTimer >= 0.0f)
        {
            _crouchTimer -= Time.deltaTime;
            Debug.Log(_crouchTimer);   
        }
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
        if (_context.ReleasedCrouch)
        {
            _crouchTimer = _context.CROUCH_COOLDOWN;
        }
        if (_context.PressedCrouch && _crouchTimer <= 0.0f)
        {
            SwitchState(_factory.AirCrouch());
        }
    }
}
