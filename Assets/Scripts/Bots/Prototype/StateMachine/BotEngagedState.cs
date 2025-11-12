using UnityEngine;

public class BotEngagedState : BotBaseState
{
    float timeToForgetEnemy;
    float bHoppingDistance = 10;
    Transform enemyInSight;
    float distanceToEnemy;

    public BotEngagedState(BotStateMachine stateMachine, BotStateFactory stateFactory) : base(stateMachine, stateFactory) 
    {
        name = "Engaged";
        isRootState = true;

        //InitializeSubState();
    }

    public override void EnterState() 
    {
        timeToForgetEnemy = botStateMachine.TimeToForgetEnemy;
    }
    public override void UpdateState()
    {
        enemyInSight = botStateMachine.Vision.EnemyInSight();
        botStateMachine.UpdateAgentDestination(botStateMachine.PriorityEnemy.position);
        if (enemyInSight == null)
        {
            timeToForgetEnemy -= Time.deltaTime;
        }
        else
        {
            timeToForgetEnemy = botStateMachine.TimeToForgetEnemy;
        }

        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        distanceToEnemy = Vector3.Distance(botStateMachine.transform.position, botStateMachine.PriorityEnemy.position);
        if (distanceToEnemy <= botStateMachine.ComfortDistanceToEnemy)
        {
            botStateMachine.AddForce(-botStateMachine.transform.forward * (botStateMachine.Speed / 2), ForceMode.Force);
        }
    }

    public override void ExitState() 
    {
        
    }

    public override void CheckSwitchStates() 
    { 
        if (timeToForgetEnemy <= 0)
        {
            SwitchState(botStateFactory.Patrol());
        }
        else if (distanceToEnemy >= botStateMachine.EngageDistance)
        {
            SwitchState(botStateFactory.Chasing());
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
