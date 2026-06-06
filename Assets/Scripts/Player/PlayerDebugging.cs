using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerDebugging : MonoBehaviour, Subscriber
{
    [Header("Settings")]
    [SerializeField] bool logLinearVelocity;
    LinkedList<string> stateHistory;
    TextMeshProUGUI speedTracker;
    TextMeshProUGUI stateTracker;
    Rigidbody rb;

    PlayerStateMachine playerStateMachine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speedTracker = GameObject.Find("Speed Tracker").GetComponent<TextMeshProUGUI>();
        stateTracker = GameObject.Find("State Tracker").GetComponent<TextMeshProUGUI>();
        stateHistory = new LinkedList<string>();
        FindFirstObjectByType<GameStateManager>().AddSubscriber(this);     
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
        Debug.Log(GetStateHistory());
    }

    string GetStateHistory()
    {
        PlayerBaseState activeState = playerStateMachine.CurrentState.GetActiveState();
        return activeState.StateName;
        // string history = "";
        // foreach (string state in stateHistory)
        // {
        //     history += state + " > ";
        // }
        // return history;
    }

    public void notify(EventMessage message)
    {
        switch (message.title)
        {
            case GameStateMessages.PLAYER_ENTER_STATE_MESSAGE_TITLE:
                string stateName = (string)message.arguments[0];
                stateHistory.AddLast(stateName);
                break;
            case GameStateMessages.PLAYER_EXIT_STATE_MESSAGE_TITLE:
                if(stateHistory.Count > 1)            
                    stateHistory.RemoveLast();
                break;
        }
    }
}