using UnityEngine;

public class PlayerRollState : PlayerBaseState
{
    float ROLL_SPEED;
    float _rollDuration = .45f;
    public PlayerRollState(PlayerStateMachine context, PlayerStateFactory factory)
    : base(context, factory)
    {
        name = "Roll";
        _context.OrientationAnimator.SetBool("Rolling", true);
        _context.CurrentDrag = _context.CrouchDrag;
        ROLL_SPEED = _context.GroundSpeed;
    }
    
    public override void EnterState(){}
    public override void UpdateState()
    {
        _rollDuration -= Time.deltaTime;
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
        Move();
    }
    public override void ExitState()
    {
        _context.OrientationAnimator.SetBool("Rolling", false);
    }
    public override void CheckSwitchStates()
    {
        if (_rollDuration <= 0)
        {
            SwitchState(_factory.Move());
        }
    }
    public override void InitializeSubState(){}

    void Move()
    {
        Transform orientation = _context.PlayerOrientation;
        Vector3 directionVector = orientation.forward;
        Rigidbody rb = _context.PlayerRigidBody;
        
        rb.AddForce(directionVector * ROLL_SPEED);
    }
}
