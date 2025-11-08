using UnityEngine;

public class BotMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    [Header ("Movement Settings")]
    [SerializeField] float groundSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float airMultiplier = .5f;
    [SerializeField] float maxGroundSpeed;
    [SerializeField] float maxAirSpeed;
    [SerializeField] float drag;

    Rigidbody rb;
    Transform orientation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        orientation = transform.Find("Orientation");
    }

    public void Move(float horizontal, float vertical)
    {
        float multiplier = Grounded() ? 1 : airMultiplier;
        Vector3 directionVector = orientation.forward * vertical + orientation.right * horizontal;

        rb.AddForce(directionVector.normalized * groundSpeed * multiplier);
        rb.linearDamping = Grounded() ? drag : 0;

        LimitSpeed();
    }

    public void Jump()
    {
        if (!Grounded()) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void LimitSpeed()
    {
        float speedLimit = Grounded() ? maxGroundSpeed : maxAirSpeed;
        Vector3 xzVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (xzVelocity.magnitude > maxGroundSpeed)
        {
            xzVelocity = xzVelocity.normalized * speedLimit;
            rb.linearVelocity = new Vector3(xzVelocity.x, rb.linearVelocity.y, xzVelocity.z);
        }
    }

    bool Grounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheck.GetComponent<SphereCollider>().radius, groundLayer);
    }
}
