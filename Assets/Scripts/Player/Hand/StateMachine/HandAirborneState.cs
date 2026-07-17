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
    public override void ExitState()
    {
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
        SetSubState(_factory.AirMove());
    }

    public override void FixedUpdateState(){}
}
