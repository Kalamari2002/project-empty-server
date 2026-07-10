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
    }
    public override void CheckSwitchStates()
    {
        if (!_context.IsCrouchPressed)
        {
            _context.OrientationAnimator.Play("AirStandUp");
            _context.OrientationAnimator.SetBool("AirCrouched", false);
            SwitchState(_factory.Freefall());
        }
    }
    public override void InitializeSubState(){}

    void Crouch()
    {
        Animator animator = _context.OrientationAnimator;
        animator.Play("AirCrouch");
        animator.SetBool("AirCrouched", true);
    }
}
