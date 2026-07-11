/**
* If you're surprised by the sheer amount of nothing happening
* in this state, it's cause the actual crouching is handled by
* the animator.
*/
public class PlayerAirCrouchState : PlayerBaseState
{
    public PlayerAirCrouchState(PlayerStateMachine context, PlayerStateFactory factory)
    :base(context, factory)
    {
        name = "AirCrouch";
    }

    public override void EnterState(){}
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState(){}
    public override void ExitState(){}
    public override void CheckSwitchStates()
    {
        if (!_context.IsCrouchPressed)
        {
            SwitchState(_factory.Freefall());
        }
    }
    public override void InitializeSubState(){}
}
