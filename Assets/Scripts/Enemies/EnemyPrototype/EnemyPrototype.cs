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
    Transform Player;
    CharacterController controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (Player == null) { return; }

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
