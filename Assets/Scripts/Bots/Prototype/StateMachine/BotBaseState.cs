using UnityEngine;

public class BotBaseState : BaseState
{
    protected BotStateMachine botStateMachine;
    protected BotStateFactory botStateFactory;

    public BotBaseState(BotStateMachine stateMachine, BotStateFactory stateFactory) : base(stateMachine, stateFactory)
    {
        botStateMachine = stateMachine;
        botStateFactory = stateFactory;
    }

    public override void CheckSwitchStates()
    {
        throw new System.NotImplementedException();
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
