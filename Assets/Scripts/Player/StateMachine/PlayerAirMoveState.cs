using UnityEngine;

public class PlayerAirMoveState : PlayerBaseState
{
    public PlayerAirMoveState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory)
    {
        StateName = "AirMove";
        InitializeSubState();
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
        if(_context.IsCrouchPressed)
            SetSubState(_factory.AirCrouch());
        else
            SetSubState(_factory.Freefall());
        _currentSubState.EnterState();
    }
}
