using UnityEngine;

public class BotPatrolingState : BotBaseState
{
    Transform enemyInSight;
    float minTimeToJump = 5;
    float maxTimeToJump = 10;
    float timeToJump;

    public BotPatrolingState(BotStateMachine stateMachine, BotStateFactory stateFactory) : base(stateMachine, stateFactory) 
    {
        name = "Patroling";
        isRootState = true;
        timeToJump = Random.Range(minTimeToJump, maxTimeToJump);
        //InitializeSubState();
    }

    public override void EnterState() 
    {
        botStateMachine.SetPriorityEnemy(null);
        botStateMachine.GetNextHotSpot();
    }
    public override void UpdateState()
    {
        timeToJump -= Time.deltaTime;
        botStateMachine.UpdateAgentDestination(botStateMachine.CurrentHotSpotDestination);
        if (Vector3.Distance(botStateMachine.transform.position, botStateMachine.CurrentHotSpotDestination) <= botStateMachine.StoppingDistance)
        {
            botStateMachine.GetNextHotSpot();
        }

        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
        if (timeToJump <= 0)
        {
            botStateMachine.Jump();
            timeToJump = Random.Range(minTimeToJump, maxTimeToJump);
        }
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
            float distanceToEnemy = Vector3.Distance(botStateMachine.transform.position, enemyInSight.position);
            SwitchState(distanceToEnemy < botStateMachine.EngageDistance ? botStateFactory.Engage() : botStateFactory.Chasing());
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
