/**
* CONTEXT
* Contains variables needed for different states to work
*/
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
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
    [SerializeField] float crouchDrag;
    [SerializeField] float slideDrag;

    float currDrag;

    Rigidbody rb;
    Transform orientation;

    PlayerAim playerAim;

    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    public PlayerBaseState CurrentState { get{return _currentState;} set{_currentState = value;} }

    public Rigidbody PlayerRigidBody { get{ return rb; } }
    public Transform PlayerOrientation { get{ return orientation; } }
    public bool Grounded { get{ return Physics.CheckSphere(groundCheck.position, groundCheck.GetComponent<SphereCollider>().radius, groundLayer); }}
    public bool PressedJump { get { return Input.GetKeyDown(KeyCode.Space); } } 
    public bool IsDirectionPressed { get { return Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0; } }
    public float JumpForce { get{ return jumpForce; } } 
    public float AirMultiplier { get{ return airMultiplier; } }
    public float CurrentDrag { get{ return currDrag; } }
    public float GroundSpeed { get{ return groundSpeed; } }
    public float AirSpeed { get{ return airSpeed; } }
    public float MaxGroundSpeed{ get{ return maxGroundSpeed; } }
    public float MaxAirSpeed{ get{ return maxAirSpeed; } }
    public float HorizontalInput { get{ return Input.GetAxisRaw("Horizontal"); } }
    public float VerticalInput { get{ return Input.GetAxisRaw("Vertical"); } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAim = GetComponent<PlayerAim>();
        rb = GetComponent<Rigidbody>();
        orientation = transform.Find("Orientation");
        
        currDrag = drag;

        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateStates();
    }
    void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
    }

    void Jump()
    {
        if (!Grounded) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {   
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

}
