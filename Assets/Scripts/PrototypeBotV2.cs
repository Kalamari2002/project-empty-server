using UnityEngine;

public class PrototypeBotV2 : PrototypeCharacterBase
{
    [SerializeField] float shootRate = 0.5f;
    [SerializeField] float timeToShoot;

    BotStateMachine botStateMachine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        botStateMachine = GetComponent<BotStateMachine>();
        timeToShoot = shootRate;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Respawn()
    {
        base.Respawn();
        botStateMachine.CurrentState.ExitState();
        botStateMachine.CurrentState = botStateMachine.StateFactory.Patrol();
        botStateMachine.CurrentState.EnterState();
    }

    public void CountDownToShoot()
    {
        timeToShoot -= Time.deltaTime;
        if (timeToShoot <= 0)
        {
            Shoot();
            timeToShoot = shootRate;
        }
    }
}
