using UnityEngine;

public class TestElevator : MonoBehaviour
{

    [SerializeField] GameObject topInsideZone;
    [SerializeField] GameObject bottomInsideZone;

    bool blocked = true;
    bool canTeleportPlayer = true;
    bool playedUnblockAnimation = false;
    float checkIfBlockedInitialTime = 7;
    float checkIfBlockedTime;
    Animator animator;
    GameObject player;

    public bool Blocked {  get { return blocked; } set {  blocked = value; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        checkIfBlockedTime = checkIfBlockedInitialTime;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (blocked || !playedUnblockAnimation)
        {
            checkIfBlockedTime -= Time.deltaTime;
            if (checkIfBlockedTime <= 0)
            {
                if (blocked)
                {
                    animator.Play("TestElevatorBlocked", -1, 0);
                    checkIfBlockedTime = checkIfBlockedInitialTime;
                }
                else
                {
                    animator.Play("TestElevatorOpenBottom", -1, 0);
                    playedUnblockAnimation = true;
                }
            }
        }
    }

    public void StartTeleportPlayer(bool isTopElevator)
    {
        if (!canTeleportPlayer) return;
        canTeleportPlayer = false;
        animator.Play("TestElevatorOpen" + (isTopElevator ? "Bottom" : "Top"), -1, 0);
    }

    void EnablePlayerTeleport()
    {
        canTeleportPlayer = true;
    }

    void TeleportPlayer()
    {
        if (topInsideZone.GetComponent<InsideZone>().PlayerInside)
        {
            player.transform.position = bottomInsideZone.transform.position;
        }
        else if (bottomInsideZone.GetComponent<InsideZone>().PlayerInside)
        {
            player.transform.position = topInsideZone.transform.position;
        }
    }
}
