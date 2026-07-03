using UnityEngine;

public class PlayerWallRunState : PlayerBaseState
{
    const float FORWARD_FORCE = 2.0f; 
    public float AWAY_FORCE = 4f;
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

    public override void EnterState()
    {
    }
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
        if(_context.IsTouchingWall() == 0)
        {
           SwitchState(_factory.Freefall()); 
        }
    }
    public override void InitializeSubState(){}
    void WallRun()
    {
        _context.PlayerRigidBody.AddForce(Vector3.up * _context.UP_FORCE);
        Quaternion rotation = Quaternion.LookRotation(adjacentNormal, Vector3.up);
        _context.PlayerCamera.HorizontalClamp(rotation.eulerAngles.y + 30f);
    }
    void WallJump(Vector3 direction)
    {   
        Rigidbody rb = _context.PlayerRigidBody;
        rb.AddForce(wallNormal * AWAY_FORCE, ForceMode.Impulse);
        rb.AddForce(Vector3.up * (_context.UP_FORCE / 1.2f), ForceMode.Impulse);
        rb.AddForce(direction * FORWARD_FORCE, ForceMode.Impulse);
    }
    void Jump()
    {
        if(_context.PressedJump)
            WallJump(_context.Aim.cameraForward());
    }
}
