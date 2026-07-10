using UnityEngine;

public class PlayerAirCrouchState : PlayerBaseState
{
    const float AIR_CROUCH_GROUNDCHECK_Y = -0.248f;

    public PlayerAirCrouchState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory)
    {
        name = "AirCrouch";
    }

    public override void EnterState()
    {
        Crouch();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState(){}
    public override void ExitState()
    {
        // _context.StandUp();
    }
    public override void CheckSwitchStates()
    {
        if (!_context.IsCrouchPressed)
        {
            _context.OrientationAnimator.SetTrigger("AirStandUp");
            _context.OrientationAnimator.SetBool("AirCrouched", false);
            SwitchState(_factory.Freefall());
        }
    }
    public override void InitializeSubState(){}

    void Crouch()
    {
        _context.OrientationAnimator.SetTrigger("AirCrouch");
        _context.OrientationAnimator.SetBool("AirCrouched", true);
        // CapsuleCollider collision = _context.CollisionCapsule;

        // collision.height = _context.CROUCH_COLLISION_HEIGHT;
        // Transform groundCheck = _context.GroundCollision;
        // groundCheck.localPosition = new Vector3(
        //     groundCheck.localPosition.x, 
        //     AIR_CROUCH_GROUNDCHECK_Y, 
        //     groundCheck.localPosition.z
        // );

        // _context.CameraTransform.localPosition = _context.InitCameraPos;

        // collision.center = new Vector3(
        //     collision.center.x, 
        //     _context.CROUCH_COLLISION_CENTER_Y, 
        //     collision.center.z
        // );
    }
}
