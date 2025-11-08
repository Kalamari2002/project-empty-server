using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.Rendering;


/**
* Stacked vectors: provides "memory" to the agent. For eg. if you set it to 1, the
* agent only takes 1 observation before requesting an action. However, if you set it to 3,
* it'll take the current position and the previous 2 positions (or observations) and then 
* make a decision.This could help the agent to, for example, predict where the target will be
* based on its previous positions.
*/

public class MoveToTarget : Agent
{
    [SerializeField] int maxStep;
    [SerializeField] Transform targetTransform;
    [SerializeField] float speed;
    [SerializeField] MeshRenderer floorRenderer;
    [SerializeField] Color failureColor;
    [SerializeField] Color successColor;

    int steps = 0;
    Vector3 startingPos;
    int actionCount = 0;

    void Start()
    {
        transform.localPosition = new Vector3(Random.Range(-8.0f, 16.0f), 1.36f, Random.Range(-20f, 5.0f));
        targetTransform.localPosition = new Vector3(Random.Range(-8.0f, 16.0f), 0, Random.Range(-20f, 5.0f));
    }

    /**
    *   When the task begins again, we'll reset the position of the player
    */
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-8.0f, 16.0f), 1.36f, Random.Range(-20f, 5.0f));
        targetTransform.localPosition = new Vector3(Random.Range(-8.0f, 16.0f), 0, Random.Range(-20f, 5.0f));
    }
    void FixedUpdate()
    {
        if(steps >= 0)
            steps++;
        if(steps >= 500)
        {
            Debug.Log("Frames: " + steps + ", Action Count: " + actionCount);
            steps = -1;
        }
    }
    void Update()
    {
        if (StepCount == maxStep)
        {
            OnTimeOver();
        }

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
        /**
        * Continuous actions is set to 2: X and Z positions
        */
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * speed;
        actionCount++;
    }

    /**
    * Lets us take over the agent if it's in Heuristic mode for debugging purposes.
    */
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTimeOver()
    {
        Debug.Log("Time OUT");
        SetReward(-1.0f);
        EndEpisode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            SetReward(1.0f);    //Success, set reward as a positive value
            floorRenderer.material.color = successColor;
        }
        else if (other.CompareTag("Wall"))
        {
            SetReward(-1.0f);   //Failure, set reward as a negative value
            floorRenderer.material.color = failureColor;
        }
        else
        {
            return;
        }

        EndEpisode();   // This ends the "task" with either success or failure.
    }
}
