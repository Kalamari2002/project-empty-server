using UnityEngine;

public class EnemyStateFactory : BaseStateFactory
{
    EnemyStateMachine _context;

    public EnemyStateFactory(EnemyStateMachine currentContext) : base(currentContext)
    {
        _context = currentContext;
    }

    public BaseState Chase()
    {
        return new EnemyChaseState(_context, this);
    }

    public BaseState Idle()
    {
        return new EnemyIdleState(_context, this);
    }

    public BaseState Hitstun(int animationIndex)
    {
        return new EnemyHitstunState(_context, this, animationIndex);
    }

    public BaseState Ragdoll(float launchForce, float torque, Vector3 launchDirection, Vector3 spawnPoint)
    {
        return new EnemyRagdollState(_context, this, launchForce, torque, launchDirection, spawnPoint);
    }

    public BaseState Grabbed()
    {
        return new EnemyGrabbedState(_context, this);
    }
}
