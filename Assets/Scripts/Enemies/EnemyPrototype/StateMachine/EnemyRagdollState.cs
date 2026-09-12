using UnityEngine;

public class EnemyRagdollState : EnemyBaseState
{
    float _launchForce;
    float _torque;
    Vector3 _launchDirection;
    Vector3 _spawnPoint;

    public EnemyRagdollState(EnemyStateMachine context, EnemyStateFactory factory, float launchForce, float torque, Vector3 launchDirection, Vector3 spawnPoint)
    : base(context, factory)
    {
        name = "Ragdoll";
        isRootState = true;
        _launchForce = launchForce;
        _torque = torque;
        _spawnPoint = spawnPoint;
        _launchDirection = launchDirection;
    }
    public override void EnterState()
    {
        _context.SpawnRagdoll(_launchForce, _torque, _launchDirection, _spawnPoint);
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

    public override void CheckSwitchStates()
    {
        // The count down logic is handled by the Ragdoll object itself, see EnemyRagdoll.cs
        if (_context.RagdollCountDown <= 0)
        {
            SwitchState(_factory.Idle());
        }
    }

    public override void ExitState()
    {
        _context.transform.position = new Vector3(_context.ActiveRagdoll.transform.position.x, _context.transform.position.y, _context.ActiveRagdoll.transform.position.z);
        _context.DestroyActiveRagdoll();
        _context.SetVisibility(true);
        _context.ResetRagdollCountDown();
    }
}
