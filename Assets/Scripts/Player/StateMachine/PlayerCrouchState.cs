using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    public PlayerCrouchState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory){}
    public override void EnterState()
    {
        Debug.Log("Crouch");
        _context.CurrentDrag = _context.CrouchDrag;
                
        CapsuleCollider collision = _context.CollisionCapsule;
        collision.height = _context.CROUCH_COLLISION_HEIGHT;
        collision.center = new Vector3(
            collision.center.x, 
            -_context.CROUCH_COLLISION_CENTER_Y, 
            collision.center.z
        );
        
        Transform groundCheck = _context.GroundCollision;
        groundCheck.localPosition = new Vector3(
            groundCheck.localPosition.x, 
            _context.InitGroundCheckY, 
            groundCheck.localPosition.z
        );
        
        Transform cameraTransform = _context.CameraTransform;
        Vector3 initCameraPos = _context.InitCameraPos;
        cameraTransform.localPosition = new Vector3(
            initCameraPos.x, 
            initCameraPos.y - _context.CROUCH_CAMERA_OFFSET, 
            initCameraPos.z
        );
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState(){}
    public override void ExitState()
    {
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
    public override void CheckSwitchStates()
    {
        if (!_context.IsCrouchPressed)
        {
            SwitchState(_factory.Run());
        }
    }
    public override void InitializeSubState(){}
}
