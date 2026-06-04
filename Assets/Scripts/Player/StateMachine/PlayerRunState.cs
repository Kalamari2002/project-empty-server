using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    float _crouchTimer;
    public PlayerRunState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory){}
    public override void EnterState()
    {
        _context.OnEnterState("Run");
        _context.CurrentDrag = _context.Drag;
        _context.StandUp();
    }
    public override void UpdateState()
    {
        _context.Jump();
        CheckSwitchStates();
    }
    public override void FixedUpdateState(){}
    public override void ExitState(){}
    public override void CheckSwitchStates()
    {
        if (_context.IsCrouchPressed)
        {
            float horizontalSpeed = Vector3.Scale(
                _context.PlayerRigidBody.linearVelocity, 
                new Vector3(1,0,1)
            ).magnitude; 
            
            if(horizontalSpeed < _context.MinSpeedToSlide && _crouchTimer <= 0)
            {
                SwitchState(_factory.Crouch());
            }
        }
    }
    public override void InitializeSubState(){}

}
