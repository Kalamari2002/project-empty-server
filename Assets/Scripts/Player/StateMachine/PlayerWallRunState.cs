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
        name = "WallRun";
        RaycastHit leftHit = _context.WallRayCast(-1);
        RaycastHit rightHit = _context.WallRayCast(1);
        wallNormal = leftHit.collider ? leftHit.normal : rightHit.normal;
        adjacentNormal = Vector3.Cross(wallNormal, Vector3.up).normalized;

        SetClamp();
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
        _context.PlayerCamera.RemoveHorizontalClamp();
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

    void SetClamp()
    {
        const float CLAMP_ANGLE = 73f;
        Quaternion adjacentLookDir = Quaternion.LookRotation(adjacentNormal, Vector3.up);
        float adjacentLookAngle = adjacentLookDir.eulerAngles.y;
        float oppositeLookAngle = (adjacentLookAngle + 180f) % 360f;

        float yRotation = Mathf.Abs(_context.PlayerCamera.GetYRotation());
        if(yRotation >= oppositeLookAngle - CLAMP_ANGLE && yRotation <= oppositeLookAngle + CLAMP_ANGLE)
        {
            adjacentLookAngle = oppositeLookAngle;
        }
        // Debug.Log("LookAngle: " + adjacentLookAngle + " | Clamp Range: " + (adjacentLookAngle - CLAMP_ANGLE) + ", " + (adjacentLookAngle + CLAMP_ANGLE));
        int yRotationSign = _context.PlayerCamera.GetYRotation() < 0 ? -1 : 1;
        float clampBase = adjacentLookAngle * yRotationSign;
        _context.PlayerCamera.SetHorizontalClamp(clampBase - CLAMP_ANGLE, clampBase + CLAMP_ANGLE);   
    }
}
