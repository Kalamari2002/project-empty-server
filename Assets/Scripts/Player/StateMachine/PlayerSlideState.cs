using UnityEngine;

public class PlayerSlideState : PlayerBaseState
{
    public PlayerSlideState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory){}
    public override void EnterState()
    {
        Debug.Log("Slide");
        _context.CurrentDrag = _context.SlideDrag;
        LieDown();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState(){}
    public override void ExitState()
    {

    }
    public override void CheckSwitchStates()
    {
        //TODO: If press jump go airborne
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
        collision.height = _context.SLIDE_COLLISION_HEIGHT;
        collision.center = new Vector3(
            collision.center.x, 
            -_context.SLIDE_COLLISION_CENTER_Y, 
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
            initCameraPos.y - _context.SLIDE_CAMERA_OFFSET, 
            initCameraPos.z
        );
    }
}
