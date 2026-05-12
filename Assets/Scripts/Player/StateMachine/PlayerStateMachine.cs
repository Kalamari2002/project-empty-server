/**
* CONTEXT
* Contains variables needed for different states to work
*/
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform cameraTransform;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] CapsuleCollider collision;

    [Header ("Movement Settings")]
    [SerializeField] float _groundSpeed;
    [SerializeField] float _airSpeed;
    [SerializeField] float _jumpForce;
    [SerializeField] float _airMultiplier = .5f;
    [SerializeField] float _maxGroundSpeed;
    [SerializeField] float _maxAirSpeed;
    [SerializeField] float _drag;
    [SerializeField] float _crouchDrag;
    [SerializeField] float _slideDrag;

    float _currDrag;
    float _initGroundCheckY;
    float _initCollisionPosY;
    float _initCollisionHeight;

    Vector3 _initCameraPos;

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
    public bool IsCrouchPressed { get{ return Input.GetKey(KeyCode.LeftControl); } }
    
    public float JumpForce { get{ return _jumpForce; } } 
    public float AirMultiplier { get{ return _airMultiplier; } }
    public float CurrentDrag { get{ return _currDrag; } set{_currDrag = value;}}
    public float GroundSpeed { get{ return _groundSpeed; } }
    public float Drag { get{ return _drag; } }
    public float InitGroundCheckY { get{ return _initGroundCheckY; } }
    public float InitCollisionPosY { get {return _initCollisionPosY; } }
    public float InitCollisionHeight { get {return _initCollisionHeight; } }
    public float CrouchDrag { get{ return _crouchDrag; } }
    public float AirSpeed { get{ return _airSpeed; } }
    public float MaxGroundSpeed{ get{ return _maxGroundSpeed; } }
    public float MaxAirSpeed{ get{ return _maxAirSpeed; } }
    public float HorizontalInput { get{ return Input.GetAxisRaw("Horizontal"); } }
    public float VerticalInput { get{ return Input.GetAxisRaw("Vertical"); } }

    public float CROUCH_CAMERA_OFFSET { get { return 0.6f; } }
    public float CROUCH_COLLISION_HEIGHT { get { return 1.36367f; } }
    public float CROUCH_COLLISION_CENTER_Y { get { return 0.3181652f; } }

    public CapsuleCollider CollisionCapsule { get { return collision; } }

    public Transform GroundCollision { get { return groundCheck; } } 
    public Transform CameraTransform { get { return cameraTransform; } }
    public Vector3 InitCameraPos { get { return _initCameraPos; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAim = GetComponent<PlayerAim>();
        rb = GetComponent<Rigidbody>();
        orientation = transform.Find("Orientation");
        
        _currDrag = _drag;
        _initGroundCheckY =  groundCheck.localPosition.y;
        _initCollisionPosY = collision.center.y;
        _initCollisionHeight = collision.height;
        _initCameraPos = cameraTransform.localPosition;
        
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
            rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

}
