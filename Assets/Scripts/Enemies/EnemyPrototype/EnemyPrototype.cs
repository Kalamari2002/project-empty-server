using UnityEngine;
using UnityEngine.Video;

public class EnemyPrototype : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float backOffDistance;
    [SerializeField] float stopDistance;
    [SerializeField] VideoClip runningClip;
    [SerializeField] VideoClip idleClip;
    [SerializeField] VideoPlayer videoPlayer;

    [Header("Ragdoll")]
    [SerializeField] GameObject Ragdoll;
    [SerializeField] float ragdollLaunchForce;
    [SerializeField] float ragdollDuration;
    [SerializeField] bool ragdollMode = false;
    [SerializeField] GameObject ActiveRagdoll;
    float ragdollCountDown;

    Transform Player;
    CharacterController controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        controller = GetComponent<CharacterController>();
        ragdollCountDown = ragdollDuration;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        HandleRagdoll();
        if (Input.GetMouseButtonDown(0))
        {
            SpawnRagdoll();
        }
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

        videoPlayer.enabled = false;
        controller.enabled = false;
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
            videoPlayer.enabled = true;
            ragdollMode = false;
        }
    }

    void Move()
    {
        if (Player == null || ragdollMode) { return; }

        transform.LookAt(new Vector3(Player.position.x, transform.position.y, Player.position.z));
        if (Vector3.Distance(transform.position, Player.transform.position) >= stopDistance)
        {
            controller.SimpleMove(transform.forward * speed);
            videoPlayer.clip = runningClip;
            videoPlayer.Play();
            videoPlayer.playbackSpeed = 1;
        }
        else if (Vector3.Distance(transform.position, Player.transform.position) <= backOffDistance)
        {
            controller.SimpleMove(-transform.forward * speed/2);
            videoPlayer.clip = runningClip;
            videoPlayer.Play();
            videoPlayer.playbackSpeed = 1;
        }
        else
        {
            videoPlayer.clip = idleClip;
            videoPlayer.Play();
            videoPlayer.playbackSpeed = 0.5f;
        }
    }
}
