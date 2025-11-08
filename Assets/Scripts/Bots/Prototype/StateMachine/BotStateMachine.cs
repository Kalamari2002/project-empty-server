using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BotStateMachine : BaseStateMachine
{

    [Header("Movement Settings")]
    [SerializeField] float jumpForce;
    [SerializeField] float speed;
    [SerializeField] float linearDrag = 6;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] bool grounded;

    [Header("AI Settings")]
    [SerializeField] List<Transform> hotSpots = new List<Transform>();
    [SerializeField] List<Transform> hotSpotsRoute = new List<Transform>();
    [SerializeField] Transform priorityEnemy;
    [SerializeField] int hotSpotsRouteIndex = 0;
    [SerializeField] float pathUpdateDelay = 0.2f;
    [SerializeField] float hotSpotOffsetRange = 2;
    [SerializeField] float minDistanceToHotSpotRange = 5;
    [SerializeField] float rotationSpeed = 20;
    [SerializeField] float timeToForgetEnemy = 3;
    [SerializeField] float stoppingDistance = 5;
    [SerializeField] float comfortDistanceToEnemy = 1;
    float pathUpdateDeadline;
    NavMeshAgent navMeshAgent;
    Transform currentHotSpot;
    Vector3 currentHotSpotDestination;

    [Header("References")]
    [SerializeField] BotVision vision;

    BotStateFactory stateFactory;
    Rigidbody rb;

    //Getters and Setters
    public float MinDistanceToHotSpotRange { get { return minDistanceToHotSpotRange; } }
    public float TimeToForgetEnemy { get { return timeToForgetEnemy; } }
    public float ComfortDistanceToEnemy { get { return comfortDistanceToEnemy; } }
    public float StoppingDistance { get { return stoppingDistance; } set { stoppingDistance = value; } }
    public float Speed { get { return speed; } }
    public float LinearDrag { get { return linearDrag; } }
    public float JumpForce { get { return jumpForce; } }
    public Vector3 CurrentHotSpotDestination { get {  return currentHotSpotDestination; } set {  currentHotSpotDestination = value; } }
    public Transform CurrentHotSpot { get { return currentHotSpot; } set { currentHotSpot = value; } }
    public Transform PriorityEnemy { get { return priorityEnemy; } set {  priorityEnemy = value; } }
    public NavMeshAgent NavAgent { get { return navMeshAgent; } }
    public BotVision Vision { get { return vision; } }
    public Rigidbody Rb { get { return rb; } }

    private void Awake()
    {
        navMeshAgent = transform.Find("Navigator").GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = linearDrag;

        if (hotSpots.Count == 0)
        {
            Transform hotSpotsParent = GameObject.FindGameObjectWithTag("HotSpots").transform;
            foreach (Transform child in hotSpotsParent.GetComponentsInChildren<Transform>())
            {
                if (child != hotSpotsParent)
                {
                    hotSpots.Add(child);
                }
            }
        }

        while (hotSpots.Count > 0)
        {
            int hotSpotIndex = Random.Range(0, hotSpots.Count);
            Transform hotSpot = hotSpots[hotSpotIndex];
            hotSpots.RemoveAt(hotSpotIndex);
            hotSpotsRoute.Add(hotSpot);
        }

        stateFactory = new BotStateFactory(this);
        CurrentState = stateFactory.Patrol();
        CurrentState.EnterState();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Uncomment to debug State hierarchy on runtime, sub state switching is currently NOT working
        //Debug.Log("Super State: " + CurrentState + ", Sub State: " + CurrentState.SubState);

        grounded = Grounded();
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public Transform getClosestHotSpotTo(Vector3 target)
    {
        Transform closest = hotSpotsRoute[0];
        foreach (Transform hotSpot in hotSpotsRoute)
        {
            if (Vector3.Distance(target, hotSpot.position) < Vector3.Distance(target, closest.position))
            {
                closest = hotSpot;
            }
        }
        return closest;
    }

    public void UpdateAgentDestination(Vector3 destination)
    {
        if (Time.time >= pathUpdateDeadline)
        {
            RotateTowards(navMeshAgent.steeringTarget);
            navMeshAgent.SetDestination(destination);
            pathUpdateDeadline = Time.time + pathUpdateDelay;
        }
    }

    public void RotateTowards(Vector3 target)
    {
        Vector3 lookDirection = new Vector3(target.x, 0, target.z) - new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation
            = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);
    }

    public void GetNextHotSpot()
    {
        hotSpotsRouteIndex++;
        hotSpotsRouteIndex %= hotSpotsRoute.Count;
        currentHotSpot = hotSpotsRoute[hotSpotsRouteIndex];
        Vector3 newHotSpotPosition = currentHotSpot.position;
        currentHotSpotDestination = new Vector3(newHotSpotPosition.x + Random.Range(-hotSpotOffsetRange, hotSpotOffsetRange),
           newHotSpotPosition.y,
           newHotSpotPosition.z + Random.Range(-hotSpotOffsetRange, hotSpotOffsetRange));
    }

    public void AddForce(Vector3 direction, ForceMode forceMode)
    {
        rb.AddForce(direction, forceMode);
    }

    public void SetPriorityEnemy(Transform enemy)
    {
        priorityEnemy = enemy;
    }

    public bool Grounded()
    {
        Transform groundCheck = transform.Find("GroundCheck");
        return Physics.CheckSphere(groundCheck.position, groundCheck.GetComponent<SphereCollider>().radius, GroundLayer);
    }
}
