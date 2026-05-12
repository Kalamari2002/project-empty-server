using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory){}
    public override void EnterState()
    {
        Debug.Log("Run");
        _context.CurrentDrag = _context.Drag;
        CapsuleCollider collision = _context.CollisionCapsule;

        collision.center = new Vector3(collision.center.x, _context.InitCollisionPosY, collision.center.z);
        collision.height = _context.InitCollisionHeight;

        Transform cameraTransform = _context.CameraTransform;
        cameraTransform.localPosition = _context.InitCameraPos;

        Transform groundCheck = _context.GroundCollision;
        groundCheck.localPosition = new Vector3(
            groundCheck.localPosition.x, 
            _context.InitGroundCheckY, 
            groundCheck.localPosition.z
        );
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState(){}
    public override void ExitState(){}
    public override void CheckSwitchStates()
    {
        if (_context.IsCrouchPressed)
        {
            SwitchState(_factory.Crouch());
        }
    }
    public override void InitializeSubState(){}
}
