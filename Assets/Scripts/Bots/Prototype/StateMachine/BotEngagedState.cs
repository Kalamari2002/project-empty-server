using UnityEngine;
using UnityEngine.AI;

public class BotEngagedState : BotBaseState
{

    public BotEngagedState(BotStateMachine stateMachine, BotStateFactory stateFactory) : base(stateMachine, stateFactory) { }

    public override void EnterState() { }
    public override void UpdateState() 
    {
        CheckSwitchStates();
        UpdatePath();
    }
    public override void ExitState() 
    {
        botStateMachine.CurrentState = botStateFactory.Patrol();
        botStateMachine.CurrentState.EnterState();
    }
    public override void CheckSwitchStates() 
    { 
        if (botStateMachine.PriorityEnemy == null)
        {
            ExitState();
        }
    }
    public override void InitializeSubState() { }

    void UpdatePath()
    {
        if (Time.time >= botStateMachine.PathUpdateDeadline)
        {
            botStateMachine.PathUpdateDeadline = Time.time + botStateMachine.PathUpdateDelay;
            if (NavMesh.SamplePosition(botStateMachine.PriorityEnemy.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                botStateMachine.NavAgent.SetDestination(hit.position);
            }
            else
            {
                botStateMachine.NavAgent.SetDestination(botStateMachine.PriorityEnemy.position);
            }
        }
    }
}
