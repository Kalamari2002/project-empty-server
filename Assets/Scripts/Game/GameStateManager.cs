using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        Debug.Log("Game Started");
    }
}
