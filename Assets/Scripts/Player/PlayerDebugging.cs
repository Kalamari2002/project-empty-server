using TMPro;
using UnityEngine;

public class PlayerDebugging : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool logLinearVelocity;
    TextMeshProUGUI speedTracker;
    TextMeshProUGUI stateTracker;
    Rigidbody rb;

    PlayerStateMachine playerStateMachine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speedTracker = GameObject.Find("Speed Tracker").GetComponent<TextMeshProUGUI>();
        stateTracker = GameObject.Find("State Tracker").GetComponent<TextMeshProUGUI>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        speedTracker.text = rb.linearVelocity.magnitude.ToString();
        
        if (logLinearVelocity )
        {
            Debug.Log(rb.linearVelocity.magnitude);
        }

        stateTracker.text = GetStateHistory();
        // Debug.Log(GetStateHistory());
    }

    string GetStateHistory()
    {
        BaseState activeState = playerStateMachine.CurrentState.GetDeepestActiveState();
        return activeState.ToString();
    }
}