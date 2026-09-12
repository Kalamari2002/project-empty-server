using UnityEngine;

public class EnemyStateMachine : BaseStateMachine
{
    [Header("Settings")]
    [SerializeField] int health = 50;
    [SerializeField] float chaseSpeed;
    [SerializeField] float backOffDistance;
    [SerializeField] float stopDistance;
    [SerializeField] Animator animator;

    [Header("Ragdoll")]
    [SerializeField] GameObject ragdoll;
    [SerializeField] float ragdollLaunchForce;
    [SerializeField] float ragdollDuration;
    GameObject activeRagdoll;
    float ragdollCountDown;

    Transform player;
    CharacterController controller;
    CapsuleCollider capsuleCollider;
    SpriteRenderer spriteRenderer;

    EnemyStateFactory _states;

    public Transform Player { get { return player; } }
    public Animator Animator { get { return animator; } }
    public GameObject ActiveRagdoll { get { return activeRagdoll; } set { activeRagdoll = value; } }
    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } }
    public CapsuleCollider CapsuleCollider { get { return capsuleCollider; } }
    public CharacterController Controller { get { return controller; } }
    public float ChaseSpeed { get { return chaseSpeed; } }
    public float StopDistance { get { return stopDistance; } }
    public float RagdollCountDown { get {  return ragdollCountDown; } set { ragdollCountDown = value; } }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        ragdollCountDown = ragdollDuration;

        _states = new EnemyStateFactory(this);
        CurrentState = _states.Idle();
        CurrentState.EnterState();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void MoveTowards(Transform target, Vector3 direction, float speed)
    {
        if (target == null) {
            return;
        }

        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        controller.SimpleMove(direction * speed);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void EnterHitstun( int stunAnimation)
    {
        CurrentState.InterruptState(_states.Hitstun(stunAnimation));
    }

    public void EnterRagdoll(float launchForce, float torque, Vector3 launchDirection)
    {
        EnterRagdoll(launchForce, torque, launchDirection, transform.position);
    }

    public void EnterRagdoll(float launchForce, float torque, Vector3 launchDirection, Vector3 spawnPoint)
    {
        CurrentState.InterruptState(_states.Ragdoll(launchForce, torque, launchDirection, spawnPoint));
    }

    public void EnterGrabbed()
    {
        CurrentState.InterruptState(_states.Grabbed());
    }

    public void DestroyActiveRagdoll()
    {
        if (activeRagdoll != null)
        {
            Destroy(activeRagdoll);
            activeRagdoll = null;
        }
    }

    public void ResetRagdollCountDown()
    {
        ragdollCountDown = ragdollDuration;
    }

    public void SetVisibility(bool visible)
    {
        spriteRenderer.enabled = visible;
        controller.enabled = visible;
        capsuleCollider.enabled = visible;
    }

    public void SpawnRagdoll(float launchForce, float torque, Vector3 launchDirection, Vector3 spawnPoint)
    {
        if (activeRagdoll == null)
        {
            activeRagdoll = Instantiate(ragdoll, spawnPoint, Quaternion.identity);
            activeRagdoll.GetComponent<EnemyRagdoll>().SetUp(this);
            activeRagdoll.transform.forward = -launchDirection;
        }
        Rigidbody ragdollRb = activeRagdoll.GetComponent<Rigidbody>();
        ragdollRb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
        ragdollRb.AddTorque((Player.transform.forward + Vector3.up).normalized * torque, ForceMode.Impulse);
    }
}
