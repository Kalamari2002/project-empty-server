using UnityEngine;

public class BotChasingState : BotBaseState
{
    float timeToForgetEnemy;
    float bHoppingDistance = 10;
    float distanceToEnemy;
    Transform enemyInSight;

    public BotChasingState(BotStateMachine stateMachine, BotStateFactory stateFactory) : base(stateMachine, stateFactory)
    {
        name = "Chasing";
        isRootState = true;

        //InitializeSubState();
    }

    public override void EnterState()
    {
        timeToForgetEnemy = botStateMachine.TimeToForgetEnemy;
    }
    public override void UpdateState()
    {
        distanceToEnemy = Vector3.Distance(botStateMachine.transform.position, botStateMachine.PriorityEnemy.position);
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
        botStateMachine.AddForce(botStateMachine.transform.forward * botStateMachine.Speed, ForceMode.Force);
        if (botStateMachine.Grounded())
        {
            botStateMachine.Jump();
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
        else if (distanceToEnemy < botStateMachine.EngageDistance)
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
