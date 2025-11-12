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
        if (!botStateMachine.Grounded())
        {
            SwitchState(botStateFactory.Jump());
        }
    }

    public override void EnterState()
    {
        botStateMachine.Rb.linearDamping = botStateMachine.LinearDrag;
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
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    botStateMachine.Rb.linearVelocity = new Vector3(botStateMachine.Rb.linearVelocity.x, 0, botStateMachine.Rb.linearVelocity.z);
        //    botStateMachine.Rb.linearDamping = 0;
        //    botStateMachine.Rb.AddForce(Vector3.up * botStateMachine.JumpForce, ForceMode.Impulse);
        //}
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        
    }
}
