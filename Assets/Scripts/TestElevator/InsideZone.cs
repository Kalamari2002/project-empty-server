using UnityEngine;

public class InsideZone : MonoBehaviour
{
    [SerializeField] bool isTopZone = true;

    TestElevator testElevatorAnimator;

    bool playerInside = false;

    public bool PlayerInside { get { return playerInside; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        testElevatorAnimator = transform.parent.parent.GetComponent<TestElevator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInside = true;
            testElevatorAnimator.StartTeleportPlayer(isTopZone);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInside = false;
        }
    }
}
