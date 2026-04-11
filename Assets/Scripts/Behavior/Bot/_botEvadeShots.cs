using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

[RequireComponent(typeof(BotMovement))]

public class _botEvadeShots : Agent
{
    const float MAX_HEALTH = 125.0f;
    const float BASE_DAMAGE = 12.0f;
    const float INIT_TIMER = 8.0f;

    float INIT_ACTION_COOLDOWN = .1f;
    float remainingHealth;

    float timer;
    float actionCoolDown;
    float xInput, zInput = 0.0f;

    [SerializeField] int maxStep;
    [SerializeField] Transform targetTransform;
    [SerializeField] MeshRenderer floorRenderer;
    [SerializeField] Color failureColor;
    [SerializeField] Color successColor;

    BotMovement movement;

    void Start()
    {
        movement = GetComponent<BotMovement>();
    }

    /**
    *   When the task begins again, we'll reset the position of the player
    */
    public override void OnEpisodeBegin()
    {
        timer = INIT_TIMER;
        actionCoolDown = INIT_ACTION_COOLDOWN;

        remainingHealth = MAX_HEALTH;
        SetReward(remainingHealth);

        transform.localPosition = new Vector3(Random.Range(-21.0f, 27.6f), 1.36f, Random.Range(-32.0f, 16.0f));
        targetTransform.localPosition = new Vector3(Random.Range(-21.0f, 27.6f), 2.3f, Random.Range(-32.0f, 16.0f));
    }

    void Update()
    {
        if ((timer -= Time.deltaTime) <= 0)
        {
            OnTimeOver();
        }
        
        Debug.Log(actionCoolDown);
        actionCoolDown -= Time.deltaTime;

    }

    void FixedUpdate()
    {
        movement.Move(xInput, zInput);
    }

    // How the agent "observes its environmnet". Like inputs for the agent. "What data does your
    // model need to solve the problem?"
    public override void CollectObservations(VectorSensor sensor)
    {
        // To solve the problem "Move towards target", our agent needs two things:

        sensor.AddObservation(transform.localPosition);          // 1. Its own position
        sensor.AddObservation(targetTransform.localPosition);    // 2. The target's position
        // TODO: Add distance

        /**
        * Important note: looks like we're passing 2 pieces of data as 2 positions, but really 
        * we're passing 6 (each position has an X,Y,Z value). So the Space Size for this 
        * "MoveToTarget" agent should be 6.
        */
    }


    /**
    * At this point, the agent has collected observations and makes a decision. It receives
    * parameters from its action space, which can be used to solve the problem at hand.
    */
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actionCoolDown > 0) return;
        /**
        * Continuous actions is set to 2: X and Z positions
        */
        int moveX = actions.DiscreteActions[0] - 1;
        int moveZ = actions.DiscreteActions[1] - 1;
        //TODO: JUMP
        xInput = moveX;
        zInput = moveZ;
        Debug.Log("ACTION");
        actionCoolDown = INIT_ACTION_COOLDOWN;
    }

    /**
    * Lets us take over the agent if it's in Heuristic mode for debugging purposes.
    */
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = (int)Input.GetAxisRaw("Horizontal");
        discreteActions[1] = (int)Input.GetAxisRaw("Vertical");
    }

    private void OnTimeOver()
    {
        if(remainingHealth > 0)
        {
            floorRenderer.material.color = successColor;
        }
        EndEpisode();
    }

    public void TakeDamage()
    {
        remainingHealth -= BASE_DAMAGE;
        if (remainingHealth <= 0)
        {
            SetReward(-MAX_HEALTH);
            EndEpisode();
            floorRenderer.material.color = failureColor;
            return;
        }
        SetReward(remainingHealth);
    }
}
