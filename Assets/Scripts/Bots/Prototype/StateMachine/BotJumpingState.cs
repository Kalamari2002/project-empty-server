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
        Debug.Log("Substate Entered: Jump State");
    }

    public override void ExitState()
    {
        botStateMachine.CurrentState = botStateFactory.Grounded();
        botStateMachine.CurrentState.EnterState();
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
