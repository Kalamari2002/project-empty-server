using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory)
    {
        name = "Run";
    }
    public override void EnterState()
    {
        _context.CurrentDrag = _context.Drag;
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
            
            if(horizontalSpeed < _context.MinSpeedToSlide)
            {
                SwitchState(_factory.Crouch());
            }
        }
    }
    public override void InitializeSubState(){}

}
