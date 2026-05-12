using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine context, PlayerStateFactory factory)
    : base(context, factory)
    {
        InitializeSubState();
    }
    
    public override void EnterState()
    {
        Debug.Log("Move");
    }
    public override void UpdateState(){}
    public override void FixedUpdateState()
    {
        Move();
    }
    public override void ExitState(){}
    public override void CheckSwitchStates(){}
    public override void InitializeSubState()
    {
        if(_context.IsCrouchPressed)
            SetSubState(_factory.Crouch());
        else
            SetSubState(_factory.Run());
        _currentSubState.EnterState();
    }
    void Move()
    {
        float horizontal = _context.HorizontalInput;
        float vertical = _context.VerticalInput;
        Transform orientation = _context.PlayerOrientation;
        Vector3 directionVector = orientation.forward * vertical + orientation.right * horizontal;
        Rigidbody rb = _context.PlayerRigidBody;

        rb.AddForce(directionVector.normalized * _context.GroundSpeed);
    }
}
