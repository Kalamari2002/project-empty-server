using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{

    public PlayerGroundedState(PlayerStateMachine context, PlayerStateFactory factory)
    : base(context, factory)
    {
        name = "Grounded";
        isRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        _context.OrientationAnimator.SetBool("Grounded", true);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
        _context.PlayerRigidBody.linearDamping = _context.CurrentDrag;
        LimitSpeed();
    }
    public override void ExitState()
    {
        _context.OrientationAnimator.SetBool("Sliding", false);
        currentSubState?.ExitState();
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
        SetSubState(_factory.Move());
        currentSubState.EnterState();
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

