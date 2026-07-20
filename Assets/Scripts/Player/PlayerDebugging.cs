using TMPro;
using UnityEngine;

public class PlayerDebugging : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool logLinearVelocity;
    TextMeshProUGUI speedTracker;
    TextMeshProUGUI yRotationTracker;
    TextMeshProUGUI stateTracker;
    TextMeshProUGUI healthTracker;
    Rigidbody rb;

    PlayerStateMachine playerStateMachine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speedTracker = GameObject.Find("Speed Tracker").GetComponent<TextMeshProUGUI>();
        yRotationTracker = GameObject.Find("YRotation Tracker").GetComponent<TextMeshProUGUI>();
        stateTracker = GameObject.Find("State Tracker").GetComponent<TextMeshProUGUI>();
        healthTracker = GameObject.Find("Health Tracker").GetComponent<TextMeshProUGUI>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        speedTracker.text = rb.linearVelocity.magnitude.ToString();
        yRotationTracker.text = playerStateMachine.PlayerCamera.GetYRotation().ToString();
        if (logLinearVelocity )
        {
            Debug.Log(rb.linearVelocity.magnitude);
        }

        stateTracker.text = GetStateHistory();
        healthTracker.text = "Health: " + playerStateMachine.CurrentHealth.ToString();
        // Debug.Log(GetStateHistory())
    }

    string GetStateHistory()
    {
        BaseState activeState = playerStateMachine.CurrentState.GetDeepestActiveState();
        return activeState.ToString();
    }
}