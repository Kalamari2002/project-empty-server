using UnityEngine;

public class PlayerAirDropKickState : PlayerBaseState
{
    public PlayerAirDropKickState(PlayerStateMachine context, PlayerStateFactory factory)
    : base(context, factory)
    {
        name = "AirMoveDropKick";
        InitializeSubState();
    }

    public override void EnterState() 
    {
        Debug.Log("AirMoveDropKick ENTERED");
        _context.DropKicking = true;
        _context.CanPunch = false;
        _context.OrientationAnimator.SetBool("DropKicking", true);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
        Move();
    }
    public override void ExitState() 
    {
        _context.OrientationAnimator.SetBool("DropKicking", false);
    }
    public override void CheckSwitchStates() 
    { 
        
    }
    public override void InitializeSubState() { }
    void Move()
    {
        float horizontal = _context.HorizontalInput;
        float vertical = _context.VerticalInput;
        float multiplier = _context.AirMultiplier * 0.1f;
        Transform orientation = _context.PlayerOrientation;
        Vector3 directionVector = orientation.forward * vertical + orientation.right * horizontal;
        Rigidbody rb = _context.PlayerRigidBody;

        rb.AddForce(directionVector.normalized * _context.AirSpeed * multiplier);
    }
}
