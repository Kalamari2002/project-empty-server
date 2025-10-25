using UnityEngine;
using UnityEngine.AI;

public class BotPatrolingState : BotBaseState
{
    public BotPatrolingState(BotStateMachine stateMachine, BotStateFactory stateFactory) : base(stateMachine, stateFactory) { }

    public override void EnterState() 
    {
        botStateMachine.GetNextHotSpot();
    }
    public override void UpdateState() 
    {
        CheckSwitchStates();
        UpdatePath();
        if (Vector3.Distance(botStateMachine.transform.position, botStateMachine.CurrentHotSpotDestination) <= botStateMachine.MinDistanceToHotSpotRange)
        {
            botStateMachine.GetNextHotSpot();
        }
    }
    public override void ExitState() 
    {
        botStateMachine.CurrentState = botStateFactory.Engage();
        botStateMachine.CurrentState.EnterState();
    }
    public override void CheckSwitchStates() 
    { 
        if (botStateMachine.PriorityEnemy != null)
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
            if (NavMesh.SamplePosition(botStateMachine.CurrentHotSpotDestination, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                botStateMachine.NavAgent.SetDestination(hit.position);
            }
            else
            {
                botStateMachine.NavAgent.SetDestination(botStateMachine.CurrentHotSpot.position);
            }
        }
    }
}
