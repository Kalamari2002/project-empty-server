using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine context, PlayerStateFactory factory)
    : base(context, factory)
    {
        InitializeSubState();
        _isRootState = true;
    }

    public override void EnterState()
    {
        Debug.Log("Grounded");
    }
    public override void UpdateState()
    {
        if (_context.PressedJump)
        {
            Jump();
        }
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
        _context.PlayerRigidBody.linearDamping = _context.Grounded ? _context.CurrentDrag : 0;
        LimitSpeed();
    }
    public override void ExitState()
    {
        
    }
    public override void CheckSwitchStates()
    {
        if (!_context.Grounded)
        {
            SwitchState(_factory.Airborne());
        }
    }
    public override void InitializeSubState()
    {
        if (_context.IsDirectionPressed)
        {
            SetSubState(_factory.Move());
        }
        else
        {
            SetSubState(_factory.Idle());
        }
    }

    void Jump()
    {
        Rigidbody rb = _context.PlayerRigidBody;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * _context.JumpForce, ForceMode.Impulse);
    }

    void LimitSpeed()
    {
        Rigidbody rb = _context.PlayerRigidBody;
        float speedLimit = _context.MaxGroundSpeed;
        Vector3 xzVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (xzVelocity.magnitude > speedLimit)
        {
            xzVelocity = xzVelocity.normalized * speedLimit;
            rb.linearVelocity = new Vector3(xzVelocity.x, rb.linearVelocity.y, xzVelocity.z);
        }
    }
}

