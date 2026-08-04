/**
* CONTEXT
* Contains variables needed for different states to work.
* Rule of thumb: 
* Include a variable in the player context if:
*   1. It is used in more than one state and/or in the context.
*   2. A state is responsible for signaling an exception to another state 
*      (e.g. Airborne state determines whether a roll was successful before Roll comes in).
* Keep a variable local to a state if:
*   1. It is only used in that state, unless you want to keep it Serialized for debugging purposes.
*/
using UnityEngine;
public class PlayerStateMachine : BaseStateMachine
{

    [Header("References")]
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform wallCheckOrigin;
    [SerializeField] LayerMask wallLayers;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Animator orientationAnimator;

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

    [Header ("Combat Settings")]
    [SerializeField] float _kickLaunchForce = 5;
    [SerializeField] float _maxKickChargeTime = 1;
    float _kickChargeTime = 0;
    bool _canPunch = true;
    bool _dropKicking = false;

#region Components
    // Animator
    public Animator OrientationAnimator { get { return orientationAnimator; } }

    // Rigidbody
    Rigidbody rb;
    public Rigidbody PlayerRigidBody { get{ return rb; } }

    // Scripts
    PlayerCamera playerCamera;
    PlayerAim playerAim;
    public PlayerCamera PlayerCamera { get { return playerCamera; } }
    public Transform CameraTransform { get { return cameraTransform; } }
    public PlayerAim Aim { get { return playerAim; } }

    // Transforms
    Transform orientation;
    public Transform PlayerOrientation { get{ return orientation; } }
    public Transform WallCheckOrigin { get { return wallCheckOrigin; } }
#endregion

#region Health
    [Header ("Helth")]
    [SerializeField] int _maxHealth = 100;
    int _currentHealth;
    public int CurrentHealth { get { return _currentHealth; } }
#endregion

#region Input Checks
    // Crouch
    public bool IsCrouchPressed { get{ return Input.GetKey(KeyCode.LeftControl); } }
    public bool PressedCrouch { get { return Input.GetKeyDown(KeyCode.LeftControl); } }

    // Directions
    public bool IsDirectionPressed { get { return Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0; } }
    public float HorizontalInput { get{ return Input.GetAxisRaw("Horizontal"); } }
    public float VerticalInput { get{ return Input.GetAxisRaw("Vertical"); } }

    // Jump
    public bool PressedJump { get { return Input.GetKeyDown(KeyCode.Space); } }
#endregion


#region Airborne Movement Variables
    public float AirMultiplier { get{ return _airMultiplier; } }
    public float AirSpeed { get{ return _airSpeed; } }
    public float MaxAirSpeed{ get{ return _maxAirSpeed; } }
#endregion

#region Grounded Movement Variables
    public float JumpForce { get{ return _jumpForce; } } 
    public float Drag { get{ return _drag; } }
    public float GroundSpeed { get{ return _groundSpeed; } }
    public float MaxGroundSpeed{ get{ return _maxGroundSpeed; } }
#endregion

#region Combat Variables
    public float KickLaunchForce { get { return _kickLaunchForce; } }
    public float KickChargeTime { get { return _kickChargeTime; } }
    public float MaxKickChargeTime { get { return _maxKickChargeTime; } }
    public bool CanPunch { get { return _canPunch; } set { _canPunch = value; } }
    public bool DropKicking { get { return _dropKicking; } set { _dropKicking = value; } }
#endregion

#region Shared Movement Variables
    float _currDrag;
    public float CurrentDrag { get{ return _currDrag; } set{_currDrag = value;}}
#endregion

#region State Machine Variables
    PlayerStateFactory _states;
#endregion


#region Context Variables
    #region Crouch
        public float CrouchDrag { get{ return _crouchDrag; } }
    #endregion

    #region Grounded
        public bool Grounded { get{ return Physics.CheckSphere(groundCheck.position, groundCheck.GetComponent<SphereCollider>().radius, groundLayer); }}
    #endregion

    #region Roll
        bool _rollOnGrounded;
        public bool RollOnGrounded { get { return _rollOnGrounded; } set { _rollOnGrounded = value; } }
    #endregion

    #region Slide
        public float SlideDrag { get { return _slideDrag; } }
        public float MinSpeedToSlide { get { return _minSpeedToSlide; } }
    #endregion

    #region Wall Run
        public float MinSpeedToWallRun { get { return _minSpeedToWallRun; } }
    #endregion
#endregion

    void Awake()
    {
        playerAim = GetComponent<PlayerAim>();
        rb = GetComponent<Rigidbody>();
        orientation = transform.Find("Orientation");
        playerCamera = GetComponentInChildren<PlayerCamera>();

        _currDrag = _drag;
        _rollOnGrounded = false;
        
        _states = new PlayerStateFactory(this);
        CurrentState = _states.Grounded();
        CurrentState.EnterState();
        Debug.Log(CurrentState);
        _currentHealth = _maxHealth;
    }

    protected override void Update()
    {
        base.Update();
        orientationAnimator.SetBool("CrouchPressed", IsCrouchPressed);
        HandleKickCharge();
    }

    void HandleKickCharge()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _kickChargeTime += Time.deltaTime;
            _kickChargeTime = Mathf.Clamp(_kickChargeTime, 0, _maxKickChargeTime);
            Debug.Log("Kick Charge Time: " + _kickChargeTime);
        }
        else
        {
            _kickChargeTime = 0;
        }
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
    
    public int IsTouchingWall(float distance)
    {
        if(WallRayCast(-1, distance).collider) return -1;
        if(WallRayCast(1, distance).collider) return 1;
        return 0;
    }
    public void AddImpulse(float magnitude, Vector3 direction)
    {
        rb.AddForce(direction * magnitude, ForceMode.Impulse);
    }
    
    public RaycastHit WallRayCast(int dir)
    {
        const float WALL_CHECK_DIST = 0.8f;

        Physics.Raycast(
            WallCheckOrigin.position,
            dir * PlayerOrientation.right,
            out RaycastHit hit,
            WALL_CHECK_DIST,
            wallLayers
        );    
        
        return hit;
    }
    
    public RaycastHit WallRayCast(int dir, float distance)
    {
        Physics.Raycast(
            WallCheckOrigin.position,
            dir * PlayerOrientation.right,
            out RaycastHit hit,
            distance,
            wallLayers
        );    
        
        return hit;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Debug.Log("Player is dead");
        }
    }
}
