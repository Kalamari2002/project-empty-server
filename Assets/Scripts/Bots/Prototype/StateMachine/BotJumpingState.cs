using UnityEngine;

public class BotJumpingState : BotBaseState
{
    public BotJumpingState(BotStateMachine stateMachine, BotStateFactory stateFactory) : base(stateMachine, stateFactory)
    {
        isRootState = false;
        name = "Jumping";
    }

    public override void CheckSwitchStates()
    {
        if (botStateMachine.Grounded())
        {
            SwitchState(botStateFactory.Grounded());
        }
    }

    public override void EnterState()
    {
        botStateMachine.Rb.linearDamping = 0;
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {

    }
}
