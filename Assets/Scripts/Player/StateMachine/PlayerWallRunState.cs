using UnityEngine;

public class PlayerWallRunState : PlayerBaseState
{
    const float FORWARD_FORCE = 2.0f; 
    const float MIN_VELOCITY_TO_KEEP_RUN = 4.0f;
    public float AWAY_FORCE = 4.0f;
    Vector3 wallNormal, adjacentNormal;
    public PlayerWallRunState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory)
    {
        StateName = "WallRun";
        RaycastHit leftHit = _context.WallRayCast(-1);
        RaycastHit rightHit = _context.WallRayCast(1);
        wallNormal = leftHit.collider ? leftHit.normal : rightHit.normal;
        adjacentNormal = Vector3.Cross(wallNormal, Vector3.up).normalized;
    }

    public override void EnterState(){}

    public override void UpdateState()
    {
        CheckSwitchStates();
        Jump();
    }
    public override void FixedUpdateState()
    {
        WallRun();
    }
    public override void ExitState()
    {
    }
    public override void CheckSwitchStates()
    {
        const float RAYCAST_REACH_TO_KEEP_RUN = 2.0f;
        float horizontalVelocity = Vector3.Scale(_context.PlayerRigidBody.linearVelocity, new Vector3(1,0,1)).magnitude;
        if(
            _context.IsTouchingWall(RAYCAST_REACH_TO_KEEP_RUN) == 0 
            || _context.IsCrouchPressed == true 
            || horizontalVelocity < MIN_VELOCITY_TO_KEEP_RUN
        )
        {
           SwitchState(_factory.AirMove()); 
        }
    }
    public override void InitializeSubState(){}
    void WallRun()
    {
        float vertical = _context.VerticalInput;
        float multiplier = _context.AirMultiplier;
        Transform orientation = _context.PlayerOrientation;
        Vector3 directionVector = orientation.forward - wallNormal;
        Rigidbody rb = _context.PlayerRigidBody;
        rb.AddForce(directionVector.normalized * _context.AirSpeed * multiplier);
        rb.AddForce(Vector3.up * _context.UP_FORCE);
    }

    void WallJump(Vector3 direction)
    {   
        Rigidbody rb = _context.PlayerRigidBody;
        rb.AddForce(wallNormal * AWAY_FORCE, ForceMode.Impulse);
        rb.AddForce(Vector3.up * (_context.UP_FORCE / 1.2f), ForceMode.Impulse);
        rb.AddForce(direction * FORWARD_FORCE, ForceMode.Impulse);
        SwitchState(_factory.AirMove()); 
    }
    void Jump()
    {
        if(_context.PressedJump)
            WallJump(_context.Aim.cameraForward());
    }
}
