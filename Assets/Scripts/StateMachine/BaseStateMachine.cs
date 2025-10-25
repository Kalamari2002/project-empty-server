using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    private BaseState currentState;

    // Getters and Setters methods
    public BaseState CurrentState { get { return currentState; } set { currentState = value; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
