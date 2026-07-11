using UnityEngine;
/**
* If you're surprised by the sheer amount of nothing happening
* in this state, it's cause the actual sliding is handled by
* the animator.
*/
public class PlayerSlideState : PlayerBaseState
{
    const float JUMP_COOLDOWN_AFTER_SLIDE = .9f;
    
    float _jumpCooldown = 0;

    public PlayerSlideState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory)
    {
        name = "Slide";
        _jumpCooldown = JUMP_COOLDOWN_AFTER_SLIDE;
    }
    ~PlayerSlideState()
    {
        ExitState();
    }
    public override void EnterState()
    {
        if(!_context.Grounded) return;
        _context.CurrentDrag = _context.SlideDrag;
        _context.OrientationAnimator.SetBool("Sliding", true);
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
    public override void ExitState()
    {
        _context.OrientationAnimator.SetBool("Sliding", false);
    }
    public override void CheckSwitchStates()
    {
        float minSpeedToStopSlide = 3.8f;
        if (!_context.IsCrouchPressed || _context.PlayerRigidBody.linearVelocity.magnitude <= minSpeedToStopSlide)
        {
            SwitchState(_factory.Move());
        }
    }
    public override void InitializeSubState(){}
}
