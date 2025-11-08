using UnityEngine;

public class BotGroundedState : BotBaseState
{
    public BotGroundedState(BotStateMachine stateMachine, BotStateFactory stateFactory) : base(stateMachine, stateFactory)
    {
        isRootState = false;
        name = "Grounded";
    }
    public override void CheckSwitchStates()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchState(botStateFactory.Jump());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Substate Entered: Grounded State");
        botStateMachine.Rb.linearVelocity = new Vector3(botStateMachine.Rb.linearVelocity.x, 0, botStateMachine.Rb.linearVelocity.z);
        botStateMachine.Rb.linearDamping = botStateMachine.LinearDrag;
    }

    public override void ExitState()
    {
        botStateMachine.Rb.linearDamping = 0;
        botStateMachine.Rb.AddForce(Vector3.up * botStateMachine.JumpForce, ForceMode.Impulse);
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
