using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    [Header ("Movement Settings")]
    [SerializeField] float groundSpeed;
    [SerializeField] float airSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float airMultiplier = .5f;
    [SerializeField] float maxGroundSpeed;
    [SerializeField] float maxAirSpeed;
    [SerializeField] float drag;

    PlayerWallRun wallRun; 
    PlayerAim playerAim;
    PlayerCrouch playerCrouch;

    Rigidbody rb;
    Transform orientation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAim = GetComponent<PlayerAim>();
        wallRun = GetComponent<PlayerWallRun>();
        playerCrouch = GetComponent<PlayerCrouch>();
        rb = GetComponent<Rigidbody>();
        orientation = transform.Find("Orientation");
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Crouch();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float multiplier = Grounded() ? 1 : airMultiplier;
        Vector3 directionVector = orientation.forward * vertical + orientation.right * horizontal;
    
        rb.AddForce(directionVector.normalized * (Grounded() ? groundSpeed : airSpeed) * multiplier);
        rb.linearDamping = Grounded() ? drag : 0;
        
        WallRun(horizontal);
        
        LimitSpeed();
    }
    void WallRun(float horizontal)
    {
        float horizontalVelocity = Vector3.Scale(rb.linearVelocity, new Vector3(1,0,1)).magnitude; 
        if (Grounded() || horizontalVelocity < wallRun.GetMinBuildUpVel())
        {
            wallRun.SetCanWallRun(false);
            wallRun.SetIsWallRunning(false);
            return;
        }

        wallRun.SetCanWallRun(true);
        int touchingWall = wallRun.IsTouchingWall();
        if(touchingWall != 0 && (horizontal == touchingWall)){
            wallRun.WallRun();
            wallRun.SetIsWallRunning(true);
        }
        else
        {
            wallRun.SetIsWallRunning(false);   
        }
    }

    void Jump()
    {
        if (!Grounded() && !wallRun.GetIsWallRunning()) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallRun.GetIsWallRunning())
            {
                wallRun.WallJump(playerAim.cameraForward());
                return;
            }
            
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            playerCrouch.Crouch();
        } else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            playerCrouch.StopCrouch();
        }
    }

    void LimitSpeed()
    {
        float speedLimit = Grounded() ? maxGroundSpeed : maxAirSpeed;
        Vector3 xzVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (xzVelocity.magnitude > speedLimit)
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
