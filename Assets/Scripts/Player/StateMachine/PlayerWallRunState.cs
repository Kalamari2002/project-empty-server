using UnityEngine;

public class PlayerWallRunState : PlayerBaseState
{
    const float FORWARD_FORCE = 3f; 
    public float AWAY_FORCE = 5f; 

    public PlayerWallRunState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory){}

    public override void EnterState()
    {
        _context.OnEnterState("WallRun");
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
    public override void ExitState(){}
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
        Debug.Log("RUNNING");
        _context.PlayerRigidBody.AddForce(Vector3.up * _context.UP_FORCE);
    }
    void WallJump(Vector3 direction)
    {
        RaycastHit leftHit = _context.WallRayCast(-1);
        RaycastHit rightHit = _context.WallRayCast(1);
        
        Vector3 wallNormal = leftHit.collider ? leftHit.normal : rightHit.normal;
        
        Rigidbody rb = _context.PlayerRigidBody;
        rb.AddForce(wallNormal * AWAY_FORCE, ForceMode.Impulse);
        rb.AddForce(Vector3.up * _context.UP_FORCE, ForceMode.Impulse);
        rb.AddForce(direction * FORWARD_FORCE, ForceMode.Impulse);
    }
    void Jump()
    {
        if(_context.PressedJump)
            WallJump(_context.Aim.cameraForward());
    }
}
