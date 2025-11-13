using UnityEngine;

public class BotEngagedState : BotBaseState
{
    float timeToForgetEnemy;
    Transform enemyInSight;
    float distanceToEnemy;
    bool goingRight = false;
    float minTimeToSwitchDirection = 1;
    float maxTimeToSwitchDirection = 1;
    float timeToSwitchDirection;

    public BotEngagedState(BotStateMachine stateMachine, BotStateFactory stateFactory) : base(stateMachine, stateFactory) 
    {
        name = "Engaged";
        isRootState = true;
        timeToSwitchDirection = Random.Range(minTimeToSwitchDirection, maxTimeToSwitchDirection);
        goingRight = (Random.Range(0, 2)) == 0 ? false : true;
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

        timeToSwitchDirection -= Time.deltaTime;
        if (timeToSwitchDirection <= 0)
        {
            goingRight = !goingRight;
            timeToSwitchDirection = Random.Range(minTimeToSwitchDirection, maxTimeToSwitchDirection);
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
        float direction = goingRight ? 1 : -1;
        botStateMachine.AddForce(botStateMachine.transform.right * direction * (botStateMachine.Speed), ForceMode.Force);
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
