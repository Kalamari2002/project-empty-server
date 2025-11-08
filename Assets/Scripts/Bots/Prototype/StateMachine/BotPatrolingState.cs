using UnityEngine;

public class BotPatrolingState : BotBaseState
{
    Transform enemyInSight;

    public BotPatrolingState(BotStateMachine stateMachine, BotStateFactory stateFactory) : base(stateMachine, stateFactory) 
    {
        name = "Patroling";
        isRootState = true;
        InitializeSubState();
    }

    public override void EnterState() 
    {
        Debug.Log("Superstate Entered: Patroling State");
        botStateMachine.GetNextHotSpot();
    }
    public override void UpdateState() 
    {
        botStateMachine.UpdateAgentDestination(botStateMachine.CurrentHotSpotDestination);
        if (Vector3.Distance(botStateMachine.transform.position, botStateMachine.CurrentHotSpotDestination) <= botStateMachine.StoppingDistance)
        {
            botStateMachine.GetNextHotSpot();
        }
        
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
        botStateMachine.AddForce(botStateMachine.transform.forward * botStateMachine.Speed, ForceMode.Force);
    }
    public override void ExitState() 
    {
        botStateMachine.SetPriorityEnemy(enemyInSight);
    }
    public override void CheckSwitchStates() 
    {
        enemyInSight = botStateMachine.Vision.EnemyInSight();
        if (enemyInSight != null)
        {
            SwitchState(botStateFactory.Engage());
        }
    }
    public override void InitializeSubState() 
    {
        if (botStateMachine.Grounded())
        {
            SetSubState(botStateFactory.Grounded());
        }
        else
        {
            SetSubState(botStateFactory.Jump());
        }
    }
}
