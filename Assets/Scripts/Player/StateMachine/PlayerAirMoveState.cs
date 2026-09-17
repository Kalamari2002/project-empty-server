using UnityEngine;

public class PlayerAirMoveState : PlayerBaseState
{
    public PlayerAirMoveState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory)
    {
        name = "AirMove";
        InitializeSubState();
    }

    public override void EnterState() 
    {
        _context.CanDropKick = true;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
        Move();
    }

    public override void ExitState()
    {
        _context.CanDropKick = false;
    }

    public override void CheckSwitchStates()
    {
        if (_context.IsTouchingWall() != 0)
        {
            float horizontalVelocity = Vector3.Scale(_context.PlayerRigidBody.linearVelocity, new Vector3(1,0,1)).magnitude;
            if(horizontalVelocity >= _context.MinSpeedToWallRun && _context.IsTouchingWall() == _context.HorizontalInput)
            {
                GameObject wall = _context.WallRayCast((int)_context.HorizontalInput).collider.gameObject;
                PlayerAirborneState superState = (PlayerAirborneState) currentSuperState;
                
                if (wall == superState.LastWallRunSurface) return;
                
                superState.LastWallRunSurface = wall;
                currentSubState.ExitState();
                SwitchState(_factory.WallRun());
            } 
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && _context.KickChargeTime >= _context.MaxKickChargeTime && !_context.GrabbingEnemy)
        {
            Debug.Log("ENTERING DROP KICK STATE");
            currentSubState.ExitState();
            SwitchState(_factory.DropKick());
        }
    }
    public override void InitializeSubState()
    {
        if(_context.IsCrouchPressed)
            SetSubState(_factory.AirCrouch());
        else
            SetSubState(_factory.Freefall());
        currentSubState.EnterState();
    }
    void Move()
    {
        float horizontal = _context.HorizontalInput;
        float vertical = _context.VerticalInput;
        float multiplier = _context.AirMultiplier;
        Transform orientation = _context.PlayerOrientation;
        Vector3 directionVector = orientation.forward * vertical + orientation.right * horizontal;
        Rigidbody rb = _context.PlayerRigidBody;

        rb.AddForce(directionVector.normalized * _context.AirSpeed * multiplier);
    }
}
