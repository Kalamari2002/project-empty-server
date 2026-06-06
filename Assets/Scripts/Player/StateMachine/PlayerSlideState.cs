using UnityEngine;

public class PlayerSlideState : PlayerBaseState
{
    const float SLIDE_CAMERA_OFFSET = 0.9f;
    const float SLIDE_COLLISION_CENTER_Y = 0.5f;
    const float JUMP_COOLDOWN_AFTER_SLIDE = .9f;
    
    float _jumpCooldown = 0;

    public PlayerSlideState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory)
    {
        StateName = "Slide";
        _jumpCooldown = JUMP_COOLDOWN_AFTER_SLIDE;
    }
    public override void EnterState()
    {
        _context.CurrentDrag = _context.SlideDrag;
        LieDown();
    }
    public override void UpdateState()
    {
        if (_jumpCooldown <= 0)
        {
            _context.Jump();
        }
        _jumpCooldown -= Time.deltaTime;
        CheckSwitchStates();
    }
    public override void FixedUpdateState(){}
    public override void ExitState(){}
    public override void CheckSwitchStates()
    {
        float minSpeedToStopSlide = 3.8f;
        if (
            !_context.IsCrouchPressed 
            || _context.PlayerRigidBody.linearVelocity.magnitude <= minSpeedToStopSlide
        )
        {
            SwitchState(_factory.Move());
        }
    }
    public override void InitializeSubState(){}

    void LieDown()
    {
        CapsuleCollider collision = _context.CollisionCapsule;
        if(collision.height < _context.InitCollisionHeight)
        {
            collision.height = _context.SLIDE_COLLISION_HEIGHT;
            return;
        }

        collision.height = _context.SLIDE_COLLISION_HEIGHT;
        collision.center = new Vector3(
            collision.center.x, 
            -SLIDE_COLLISION_CENTER_Y, 
            collision.center.z
        );
        
        Transform cameraTransform = _context.CameraTransform;
        Vector3 initCameraPos = _context.InitCameraPos;
        cameraTransform.localPosition = new Vector3(
            initCameraPos.x, 
            initCameraPos.y - SLIDE_CAMERA_OFFSET, 
            initCameraPos.z
        );
    }
}
