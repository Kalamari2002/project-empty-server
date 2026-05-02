using UnityEngine;

public class EnemyPrototype : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int health = 50;
    [SerializeField] float speed;
    [SerializeField] float backOffDistance;
    [SerializeField] float stopDistance;
    [SerializeField] Animator animator;

    [Header("Ragdoll")]
    [SerializeField] GameObject Ragdoll;
    [SerializeField] float ragdollLaunchForce;
    [SerializeField] float ragdollDuration;
    [SerializeField] bool ragdollMode = false;
    [SerializeField] GameObject ActiveRagdoll;
    float ragdollCountDown;

    bool inHitstun = false;

    Transform Player;
    CharacterController controller;
    CapsuleCollider capsuleCollider;
    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        ragdollCountDown = ragdollDuration;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        HandleRagdoll();
    }

    public void SpawnRagdoll(float launchForce, float torque)
    {
        if (ActiveRagdoll == null)
        {
            ActiveRagdoll = Instantiate(Ragdoll, transform.position, Quaternion.identity);
            ActiveRagdoll.transform.forward = transform.forward;
        }
        Rigidbody ragdollRb = ActiveRagdoll.GetComponent<Rigidbody>();
        ragdollRb.AddForce((Player.transform.forward + Vector3.up).normalized * launchForce, ForceMode.Impulse);
        ragdollRb.AddTorque((Player.transform.forward + Vector3.up).normalized * torque, ForceMode.Impulse);
        ragdollMode = true;
        ragdollCountDown = ragdollDuration;
        spriteRenderer.enabled = false;
        controller.enabled = false;
        capsuleCollider.enabled = false;
    }

    void HandleRagdoll()
    {
        if (!ragdollMode) return;
        ragdollCountDown -= Time.deltaTime;
        if (ragdollCountDown <= 0)
        {
            transform.position = new Vector3(ActiveRagdoll.transform.position.x, transform.position.y, ActiveRagdoll.transform.position.z);
            ragdollCountDown = ragdollDuration;
            Destroy(ActiveRagdoll);
            ActiveRagdoll = null;
            controller.enabled = true;
            spriteRenderer.enabled = true;
            ragdollMode = false;
            capsuleCollider.enabled = true;
        }
    }

    void Move()
    {
        if (Player == null || ragdollMode || inHitstun) { return; }

        transform.LookAt(new Vector3(Player.position.x, transform.position.y, Player.position.z));
        if (Vector3.Distance(transform.position, Player.transform.position) >= stopDistance)
        {
            controller.SimpleMove(transform.forward * speed);
            animator.SetBool("Running", true);
            animator.speed = 1;
        }
        else if (Vector3.Distance(transform.position, Player.transform.position) <= backOffDistance)
        {
            controller.SimpleMove(-transform.forward * speed/2);
            animator.SetBool("Running", true);
            animator.speed = 1;
        }
        else
        {
            animator.SetBool("Running", false);
            animator.speed = 0.5f;
        }
    }

    void ExitHitstun()
    {
        inHitstun = false;
        animator.SetBool("Hitstun", inHitstun);
    }

    public void TakeDamage(int damage)
    {
        TakeDamage(damage, 0);
    }

    public void TakeDamage(int damage, int stunAnimation)
    {
        inHitstun = stunAnimation > 0;
        animator.SetBool("Hitstun", inHitstun);
        health -= damage;
        switch(stunAnimation)
        {
            case 1:
                animator.speed = 0.5f;
                animator.Play("EnemyPrototypeHitstun1", -1, 0);
                break;
            case 2:
                animator.speed = 0.5f;
                animator.Play("EnemyPrototypeHitstun2", -1, 0);
                break;
        }
    }
}
