using System;
using UnityEngine;

public class PlayerFreefallState : PlayerBaseState
{
    float _crouchTimer;
    public PlayerFreefallState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory)
    {
        _crouchTimer = -1.0f;
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Freefall");
    }
    public override void UpdateState()
    {
        if(_crouchTimer >= 0.0f)
        {
            _crouchTimer -= Time.deltaTime;
            Debug.Log(_crouchTimer);   
        }
        Crouch();
        CheckSwitchStates();
    }
    public override void FixedUpdateState(){}
    public override void ExitState(){}
    public override void CheckSwitchStates()
    {
        if (_context.IsTouchingWall() != 0)
        {
            float horizontalVelocity = Vector3.Scale(_context.PlayerRigidBody.linearVelocity, new Vector3(1,0,1)).magnitude;
            if(horizontalVelocity >= _context.MinSpeedToWallRun && _context.IsTouchingWall() == _context.HorizontalInput)
            {
                _currentSubState.ExitState();
                SwitchState(_factory.WallRun());
            } 
        }
    }
    public override void InitializeSubState()
    {
        SetSubState(_factory.AirCrouch());
    }
    void Crouch()
    {
        if (_context.ReleasedCrouch)
        {
            _crouchTimer = _context.CROUCH_COOLDOWN;
        }
        if (_context.PressedCrouch && _crouchTimer <= 0.0f)
        {
            _currentSubState.EnterState();
        }
    }
}
