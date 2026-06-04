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
    [SerializeField] Transform wallCheckOrigin;
    [SerializeField] LayerMask wallLayers;
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
    [SerializeField] float _minSpeedToSlide;
    [SerializeField] float _minSpeedToWallRun;

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

    GameStateManager _gameStateManager;

    public PlayerBaseState CurrentState { get{return _currentState;} set{_currentState = value;} }

    public Rigidbody PlayerRigidBody { get{ return rb; } }
    public Transform PlayerOrientation { get{ return orientation; } }
    public Transform WallCheckOrigin { get { return wallCheckOrigin; } }

    public bool Grounded { get{ return Physics.CheckSphere(groundCheck.position, groundCheck.GetComponent<SphereCollider>().radius, groundLayer); }}
    public bool PressedJump { get { return Input.GetKeyDown(KeyCode.Space); } } 
    public bool IsDirectionPressed { get { return Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0; } }
    public bool IsCrouchPressed { get{ return Input.GetKey(KeyCode.LeftControl); } }
    public bool PressedCrouch { get { return Input.GetKeyDown(KeyCode.LeftControl); } }
    public bool ReleasedCrouch { get { return Input.GetKeyUp(KeyCode.LeftControl); } }
    
    public float JumpForce { get{ return _jumpForce; } } 
    public float AirMultiplier { get{ return _airMultiplier; } }
    public float CurrentDrag { get{ return _currDrag; } set{_currDrag = value;}}
    public float GroundSpeed { get{ return _groundSpeed; } }
    public float Drag { get{ return _drag; } }
    public float SlideDrag { get { return _slideDrag; } }
    public float InitGroundCheckY { get{ return _initGroundCheckY; } }
    public float InitCollisionPosY { get {return _initCollisionPosY; } }
    public float InitCollisionHeight { get {return _initCollisionHeight; } }
    public float CrouchDrag { get{ return _crouchDrag; } }
    public float AirSpeed { get{ return _airSpeed; } }
    public float MaxGroundSpeed{ get{ return _maxGroundSpeed; } }
    public float MaxAirSpeed{ get{ return _maxAirSpeed; } }
    public float MinSpeedToSlide { get { return _minSpeedToSlide; } }
    public float HorizontalInput { get{ return Input.GetAxisRaw("Horizontal"); } }
    public float VerticalInput { get{ return Input.GetAxisRaw("Vertical"); } }
    public float MinSpeedToWallRun { get { return _minSpeedToWallRun; } }

    public float CROUCH_COLLISION_HEIGHT { get { return 1.36367f; } }
    public float CROUCH_COLLISION_CENTER_Y { get { return 0.3181652f; } }
    public float CROUCH_COOLDOWN { get { return .5f; } }
    public float SLIDE_COLLISION_HEIGHT { get { return 1.0f; } } 
    public float UP_FORCE { get { return 4f; } }

    public CapsuleCollider CollisionCapsule { get { return collision; } }

    public Transform GroundCollision { get { return groundCheck; } } 
    public Transform CameraTransform { get { return cameraTransform; } }
    public Vector3 InitCameraPos { get { return _initCameraPos; } }

    public LayerMask WallLayers { get { return wallLayers; } }

    public PlayerAim Aim { get { return playerAim; } }

    void Awake()
    {
        _gameStateManager = FindFirstObjectByType<GameStateManager>();
    }

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

    public void Jump()
    {
        if (!Grounded) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {   
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    public int IsTouchingWall()
    {
        if(WallRayCast(-1).collider) return -1;
        if(WallRayCast(1).collider) return 1;
        return 0;
    }
    
    public RaycastHit WallRayCast(int dir)
    {
        const float WALL_CHECK_DIST = 0.8f;

        Physics.Raycast(
            WallCheckOrigin.position,
            dir * PlayerOrientation.right,
            out RaycastHit hit,
            WALL_CHECK_DIST,
            WallLayers
        );    
        
        return hit;
    }
    public void StandUp()
    {
        collision.center = new Vector3(collision.center.x, InitCollisionPosY, collision.center.z);
        collision.height = InitCollisionHeight;

        cameraTransform.localPosition = InitCameraPos;

        groundCheck.localPosition = new Vector3(
            groundCheck.localPosition.x, 
            InitGroundCheckY, 
            groundCheck.localPosition.z
        );
    }

    public void OnEnterState(string stateName)
    {
        _gameStateManager.PlayerEnterState(stateName);
    }

    public void OnExitState(string stateName)
    {
        // _gameStateManager.PlayerExitState(stateName);
    }

}
