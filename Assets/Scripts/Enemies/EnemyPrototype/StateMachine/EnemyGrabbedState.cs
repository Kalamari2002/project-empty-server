using UnityEngine;

public class EnemyGrabbedState : EnemyBaseState
{
    public EnemyGrabbedState(EnemyStateMachine context, EnemyStateFactory factory)
    : base(context, factory)
    {
        name = "Grabbed";
        isRootState = true;
    }
    public override void EnterState()
    {
        _context.SetVisibility(false);
    }

    public override void InitializeSubState()
    {

    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {

    }

    /**
    * We make the assumption that the Enemy will always exit the Grabbed state into the Ragdoll state, 
    * which will be switched by the context through an Interrupt. So there's no CheckSwitchStates logic
    * here because this state will only ever be exited by an Interrupt.
    */
    public override void CheckSwitchStates()
    {
        
    }

    public override void ExitState()
    {

    }
}
