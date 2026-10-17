using UnityEngine;

public class TestElevatorDetector : MonoBehaviour
{
    [SerializeField] GameObject blockingBox;
    TestElevator testElevator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        testElevator = transform.parent.GetComponent<TestElevator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == blockingBox)
        {
            testElevator.Blocked = false;
        }
    }
}
