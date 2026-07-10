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
                _context.OrientationAnimator.Play("AirStandUp");   
            }
            else
            {
                _context.OrientationAnimator.Play("GroundStandUp");   
            }
            SwitchState(_factory.Run());
        }
    }
    public override void InitializeSubState(){}

    void Crouch()
    {
        if(_context.OrientationAnimator.GetBool("AirCrouched"))
            return;
        _context.OrientationAnimator.Play("GroundCrouch");
    }
}
