using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BotStateMachine : BaseStateMachine
{

    [Header("Movement Settings")]
    [SerializeField] float jumpForce;

    [Header("AI Settings")]
    [SerializeField] List<Transform> hotSpots = new List<Transform>();
    [SerializeField] List<Transform> hotSpotsRoute = new List<Transform>();
    [SerializeField] int hotSpotsRouteIndex = 0;
    [SerializeField] float pathUpdateDelay = 0.2f;
    [SerializeField] float hotSpotOffsetRange = 2;
    [SerializeField] float minDistanceToHotSpotRange = 5;
    [SerializeField] Transform priorityEnemy;

    float pathUpdateDeadline;
    BotStateFactory stateFactory;
    Rigidbody rb;
    NavMeshAgent navMeshAgent;
    Transform currentHotSpot;
    Vector3 currentHotSpotDestination;

    //Getters and Setters
    public NavMeshAgent NavAgent { get { return navMeshAgent; } }
    public float PathUpdateDeadline { get {  return pathUpdateDeadline; } set { pathUpdateDeadline = value; } }
    public float MinDistanceToHotSpotRange { get { return minDistanceToHotSpotRange; } }
    public float PathUpdateDelay { get { return pathUpdateDelay; } }
    public Vector3 CurrentHotSpotDestination { get {  return currentHotSpotDestination; } set {  currentHotSpotDestination = value; } }
    public Transform CurrentHotSpot { get { return currentHotSpot; } set { currentHotSpot = value; } }
    public Transform PriorityEnemy { get { return priorityEnemy; } set {  priorityEnemy = value; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();

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
    void Update()
    {
        CurrentState.UpdateState();
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

    public void SetPriorityEnemy(Transform enemy)
    {
        priorityEnemy = enemy;
    }
}
