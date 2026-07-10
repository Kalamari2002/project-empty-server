using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    const float CROUCH_CAMERA_Y_OFFSET = 0.335f; 

    public PlayerCrouchState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory){}
    public override void EnterState()
    {
        name = "Crouch";
        _context.CurrentDrag = _context.CrouchDrag;
        Crouch();
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
        if (!_context.IsCrouchPressed)
        {
            if(_context.OrientationAnimator.GetBool("AirCrouched"))
            {
                _context.OrientationAnimator.SetBool("AirCrouched", false);
                _context.OrientationAnimator.SetTrigger("AirStandUp");   
            }
            else
            {
                _context.OrientationAnimator.SetTrigger("GroundStandUp");   
            }
            SwitchState(_factory.Run());
        }
    }
    public override void InitializeSubState(){}

    void Crouch()
    {
        if( _context.OrientationAnimator.GetBool("AirCrouched"))
            return;
        _context.OrientationAnimator.SetTrigger("GroundCrouch");

        // CapsuleCollider collision = _context.CollisionCapsule;
        // if(collision.height == _context.CROUCH_COLLISION_HEIGHT) // If we're already crouched, skip
        //     return;
        
        // collision.height = _context.CROUCH_COLLISION_HEIGHT;

        // bool wasAirCrouching = collision.center.y > _context.InitCollisionPosY;
        // if(wasAirCrouching) // If we were air crouching, don't adjust the groundCheck or collision capsule position
        //     return;

        // Transform groundCheck = _context.GroundCollision;
        // groundCheck.localPosition = new Vector3(
        //     groundCheck.localPosition.x, 
        //     _context.InitGroundCheckY, 
        //     groundCheck.localPosition.z
        // );
        // collision.center = new Vector3(
        //     collision.center.x, 
        //     -_context.CROUCH_COLLISION_CENTER_Y, 
        //     collision.center.z
        // );   

        // Transform cameraTransform = _context.CameraTransform;
        // Vector3 initCameraPos = _context.InitCameraPos;
        // cameraTransform.localPosition = new Vector3(
        //     initCameraPos.x, 
        //     collision.center.y + CROUCH_CAMERA_Y_OFFSET, 
        //     initCameraPos.z
        // );
    }
}
