using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDebugging : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool logLinearVelocity;

    TextMeshProUGUI speedTracker;
    TextMeshProUGUI canWallRun;
    TextMeshProUGUI isWallRunning;
    Rigidbody rb;
    PlayerWallRun wr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wr = GetComponent<PlayerWallRun>();
        speedTracker = GameObject.Find("Speed Tracker").GetComponent<TextMeshProUGUI>();
        canWallRun = GameObject.Find("CanWallRun").GetComponent<TextMeshProUGUI>();
        isWallRunning = GameObject.Find("IsWallRunning").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        speedTracker.text = rb.linearVelocity.magnitude.ToString();
        isWallRunning.text = "IsWallRunning: " + wr.GetIsWallRunning().ToString();
        canWallRun.text = "CanWallRun: " + wr.GetCanWallRun().ToString();
        
        if (logLinearVelocity )
        {
            Debug.Log(rb.linearVelocity.magnitude);
        }
    }
}
