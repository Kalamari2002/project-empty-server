using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine context, PlayerStateFactory factory)
    : base(context, factory)
    {
        name = "Move";
        InitializeSubState();
    }
    
    public override void EnterState()
    {
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
        Move();
    }
    public override void ExitState(){}
    public override void CheckSwitchStates()
    {   
        float horizontalVelocity = Vector3.Scale(_context.PlayerRigidBody.linearVelocity, new Vector3(1,0,1)).magnitude;

        if (
            _context.IsCrouchPressed 
            && _context.VerticalInput == 1.0f
            && horizontalVelocity >= _context.MinSpeedToSlide
        )
        {
            SwitchState(_factory.Slide());
        }
    }
    public override void InitializeSubState()
    {
        if(_context.IsCrouchPressed)
            SetSubState(_factory.Crouch());
        else
            SetSubState(_factory.Run());
        currentSubState.EnterState();
    }
    void Move()
    {
        float horizontal = _context.HorizontalInput;
        float vertical = _context.VerticalInput;
        Transform orientation = _context.PlayerOrientation;
        Vector3 directionVector = orientation.forward * vertical + orientation.right * horizontal;
        Rigidbody rb = _context.PlayerRigidBody;
        
        float nonForwardMultiplier = 0.65f;
        rb.AddForce(
            directionVector.normalized 
            * _context.GroundSpeed 
            * ((vertical < 0 && horizontal == 0) ? nonForwardMultiplier : 1.0f)
        );
    }
}
