using UnityEngine;

public class BotEngagedState : BotBaseState
{
    float timeToForgetEnemy;
    Transform enemyInSight;
    public BotEngagedState(BotStateMachine stateMachine, BotStateFactory stateFactory) : base(stateMachine, stateFactory) 
    {
        name = "Engaged";
        isRootState = true;

        // TODO: sub state system is currently NOT working
        //InitializeSubState();
    }

    public override void EnterState() 
    {
        Debug.Log("Superstate Entered: Engaged State");
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
        float distanceToEnemy = Vector3.Distance(botStateMachine.transform.position, botStateMachine.PriorityEnemy.position);
        if (distanceToEnemy <= botStateMachine.ComfortDistanceToEnemy)
        {
            botStateMachine.AddForce(-botStateMachine.transform.forward * (botStateMachine.Speed / 2), ForceMode.Force);
        }
        else if (distanceToEnemy >= botStateMachine.StoppingDistance)
        {
            botStateMachine.AddForce(botStateMachine.transform.forward * botStateMachine.Speed, ForceMode.Force);
        }
    }

    public override void ExitState() 
    {
        botStateMachine.SetPriorityEnemy(null);
    }
    public override void CheckSwitchStates() 
    { 
        if (timeToForgetEnemy <= 0)
        {
            SwitchState(botStateFactory.Patrol());
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
