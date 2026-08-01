using UnityEngine;

public class HandAirborneState : HandBaseState
{
    public HandAirborneState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        isRootState = true;
        StateName = "Airborne";
        InitializeSubState();
    }

    public override void EnterState()
    {

    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState() { }
    public override void ExitState()
    {
        Debug.Log("Airborne Substate called: " + currentSubState?.ToString());
        currentSubState?.ExitState();
    }
    public override void CheckSwitchStates()
    {
        if (_context.Grounded)
        {
            SwitchState(_factory.Grounded());
        }
    }
    public override void InitializeSubState()
    {
        if (_context.Grabbing)
        {
            SetSubState(_factory.Grab());
        }
        else
        {
            SetSubState(_factory.AirMove());
        }
        currentSubState.EnterState();
    }
}
