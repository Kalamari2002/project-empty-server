using UnityEngine;
using UnityEngine.Events;
using System.Collections;
public class GameStateManager : MonoBehaviour, Publisher
{
    public UnityEvent onPlayerChangeState;
    ArrayList subscribers;

    void Start()
    {
        Application.targetFrameRate = 60;
        Debug.Log("Game Started");
        onPlayerChangeState = new UnityEvent();
        subscribers = new ArrayList();
    }
    
    public void PlayerEnterState(string stateName)
    {
        EventMessage message = new EventMessage(
            GameStateMessages.PLAYER_ENTER_STATE_MESSAGE_TITLE, 
            new object[]{ stateName }
        );
        foreach (Subscriber subscriber in subscribers)
        {
            subscriber.notify(message);
        }
    }

    public void PlayerExitState(string stateName)
    {
        EventMessage message = new EventMessage(
            GameStateMessages.PLAYER_EXIT_STATE_MESSAGE_TITLE, 
            new object[]{ stateName }
        );
        foreach (Subscriber subscriber in subscribers)
        {
            subscriber.notify(message);
        }
    }

    public void AddSubscriber(Subscriber subscriber)
    {
        subscribers.Add(subscriber);
    }

    public void RemoveSubscriber(Subscriber subscriber)
    {
        subscribers.Remove(subscriber);
    }
}
