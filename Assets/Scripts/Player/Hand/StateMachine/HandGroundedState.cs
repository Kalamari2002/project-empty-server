using UnityEngine;

public class HandGroundedState : HandBaseState
{
    public HandGroundedState(HandStateMachine context, HandStateFactory factory) 
    : base(context, factory)
    {
        StateName = "Grounded";
        isRootState = true;
        Debug.Log("Hand entered Grounded state");   
        InitializeSubState();
    }

    public override void EnterState() { }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {

    }
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {
        //if (!_context.Grounded)
        //{
        //    SwitchState(_factory.Airborne());
        //}
    }
    public override void InitializeSubState()
    {
        SetSubState(_factory.Move());
        currentSubState.EnterState();
    }
}
