using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.Rendering;

[RequireComponent(typeof(BotMovement))]
public class _botMoveToTarget : Agent
{
    float INIT_ACTION_COOLDOWN = .1f;

    [SerializeField] int maxStep;
    [SerializeField] Transform targetTransform;
    [SerializeField] MeshRenderer floorRenderer;
    [SerializeField] Color failureColor;
    [SerializeField] Color successColor;

    BotMovement movement;

    int steps = 0;
    Vector3 startingPos;
    int actionCount = 0;

    float timer = 1.0f;
    float actionCoolDown;
    float xInput, zInput = 0.0f;
    void Start()
    {
        
        actionCoolDown = INIT_ACTION_COOLDOWN;
        movement = GetComponent<BotMovement>();
        transform.localPosition = new Vector3(Random.Range(-8.0f, 16.0f), 1.36f, Random.Range(-20f, 5.0f));
        targetTransform.localPosition = new Vector3(Random.Range(-8.0f, 16.0f), 0, Random.Range(-20f, 5.0f));
    }

    /**
    *   When the task begins again, we'll reset the position of the player
    */
    public override void OnEpisodeBegin()
    {
        SetReward(0.0f);
        transform.localPosition = new Vector3(Random.Range(-8.0f, 16.0f), 1.36f, Random.Range(-20f, 5.0f));
        targetTransform.localPosition = new Vector3(Random.Range(-8.0f, 16.0f), 0, Random.Range(-20f, 5.0f));
    }

    void Update()
    {
        if (StepCount == maxStep)
        {
            OnTimeOver();
        }
        Debug.Log(actionCoolDown);
        actionCoolDown -= Time.deltaTime;
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            AddReward(-.1f);
            timer = 0;
        }

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
        discreteActions[0] = (int) Input.GetAxisRaw("Horizontal");
        discreteActions[1] = (int) Input.GetAxisRaw("Vertical");
    }

    private void OnTimeOver()
    {
        Debug.Log("Time OUT");
        EndEpisode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            AddReward(3.0f);    //Success, set reward as a positive value
            floorRenderer.material.color = successColor;
        }
        else if (other.CompareTag("Wall"))
        {
            AddReward(-1.0f);   //Failure, set reward as a negative value
            floorRenderer.material.color = failureColor;
        }
        else
        {
            return;
        }

        EndEpisode();   // This ends the "task" with either success or failure.
    }
}
