using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDebugging : MonoBehaviour
{
    TextMeshProUGUI speedTracker;
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speedTracker = GameObject.Find("Speed Tracker").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        speedTracker.text = rb.linearVelocity.magnitude.ToString();
        Debug.Log(rb.linearVelocity.magnitude);
        
    }
}
