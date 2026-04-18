using UnityEngine;

public class EnemyPrototype : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int health = 50;
    [SerializeField] int damageToKnockBack = 20;
    [SerializeField] float speed;
    [SerializeField] float backOffDistance;
    [SerializeField] float stopDistance;
    [SerializeField] Animator rendererAnimator;

    [Header("Ragdoll")]
    [SerializeField] GameObject Ragdoll;
    [SerializeField] float ragdollLaunchForce;
    [SerializeField] float ragdollDuration;
    [SerializeField] bool ragdollMode = false;
    [SerializeField] GameObject ActiveRagdoll;
    float ragdollCountDown;

    Transform Player;
    CharacterController controller;
    CapsuleCollider capsuleCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        controller = GetComponent<CharacterController>();
        ragdollCountDown = ragdollDuration;
        rendererAnimator = transform.Find("SpriteRenderer").GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        HandleRagdoll();
    }

    void SpawnRagdoll()
    {
        if (ActiveRagdoll == null)
        {
            ActiveRagdoll = Instantiate(Ragdoll, transform.position, Quaternion.identity);
            ActiveRagdoll.transform.forward = transform.forward;
        }
        Rigidbody ragdollRb = ActiveRagdoll.GetComponent<Rigidbody>();
        ragdollRb.AddForce((Player.transform.forward + Vector3.up).normalized * ragdollLaunchForce, ForceMode.Impulse);
        ragdollRb.AddTorque((Player.transform.forward + Vector3.up).normalized * ragdollLaunchForce, ForceMode.Impulse);
        ragdollMode = true;
        ragdollCountDown = ragdollDuration;

        rendererAnimator.gameObject.SetActive(false);
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
            rendererAnimator.gameObject.SetActive(true);
            ragdollMode = false;
            capsuleCollider.enabled = true;
        }
    }

    void Move()
    {
        if (Player == null || ragdollMode) { return; }

        transform.LookAt(new Vector3(Player.position.x, transform.position.y, Player.position.z));
        if (Vector3.Distance(transform.position, Player.transform.position) >= stopDistance)
        {
            controller.SimpleMove(transform.forward * speed);
            rendererAnimator.SetBool("Running", true);
            rendererAnimator.speed = 1;
        }
        else if (Vector3.Distance(transform.position, Player.transform.position) <= backOffDistance)
        {
            controller.SimpleMove(-transform.forward * speed/2);
            rendererAnimator.SetBool("Running", true);
            rendererAnimator.speed = 1;
        }
        else
        {
            rendererAnimator.SetBool("Running", false);
            rendererAnimator.speed = 0.5f;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (damage >= damageToKnockBack)
        {
            SpawnRagdoll();
        }
    }
}
