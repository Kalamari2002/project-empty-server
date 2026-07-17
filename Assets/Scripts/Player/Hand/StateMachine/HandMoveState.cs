using UnityEngine;

public class HandMoveState : HandBaseState
{
    public HandMoveState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "Move";
        InitializeSubState();
    }

    public override void EnterState() 
    {
        Debug.Log("Hand entered Move state, which is NOT currently implemented");
        _context.CanPunch = true;
        _context.Animator.speed = 1.0f;
        _context.Animator.Play("HandIdle", -1, 0);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
    }
    public override void ExitState() 
    {

    }
    public override void CheckSwitchStates()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SwitchState(_factory.PunchOne());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SwitchState(_factory.Kick());
        }

        if (Input.GetMouseButtonDown(1) && _context.CastGrabHit())
        {
            SwitchState(_factory.Grab());
        }

        //if (_context.IsCrouchPressed && _context.VerticalInput == 1.0f)
        //{
        //    float horizontalVelocity = Vector3.Scale(
        //        _context.PlayerRigidBody.linearVelocity,
        //        new Vector3(1, 0, 1)
        //    ).magnitude;

        //    if (horizontalVelocity >= _context.MinSpeedToSlide)
        //    {
        //        SwitchState(_factory.Slide());
        //    }
        //}
    }
    public override void InitializeSubState()
    {
        //if (_context.IsCrouchPressed)
        //    SetSubState(_factory.Crouch());
        //else
        //    SetSubState(_factory.Run());
        //currentSubState.EnterState();
    }
}
