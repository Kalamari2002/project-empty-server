using UnityEngine;

public class HandStateMachine : BaseStateMachine
{
    [Header("General Combat Settings")]
    [SerializeField] int hitActiveFrames = 7;

    [Header("Punch Settings")]
    [SerializeField] float punchRange = 5;
    [SerializeField] int punchBaseDamage = 10;
    [SerializeField] float airPunchLaunchForce = 20;

    [Header("Kick Settings")]
    [SerializeField] float kickRange = 4;
    [SerializeField] int kickDamage = 5;
    [SerializeField] float kickLaunchForce = 5;
    [SerializeField] float maxKickChargeTime = 1;
    float _kickChargeTime;

    [Header("Grab Settings")]
    [SerializeField] float grabRange = 5;

    [Header("Feedback Settings")]
    [SerializeField] float camShakeMagnitude = 0.1f;
    [SerializeField] float camShakeDuration = 0.2f;
    [SerializeField] float hitStopDuration = 0.05f;

    bool _dropKicking = false;
    bool _grabbing = false;
    int _grabPunchAnimation = 1;

    Animator animator;
    PlayerAim playerAim;
    PlayerStateMachine playerStateMachine;
    HandStateFactory _states;
    Transform playerCameraTransform;

    public bool Grounded { get { return playerStateMachine.Grounded; } }
    public bool CanPunch { get { return playerStateMachine.CanPunch; } set { playerStateMachine.CanPunch = value; } }
    public bool DropKicking { get { return _dropKicking; } set { _dropKicking = value; } }
    public float KickLaunchForce { get { return kickLaunchForce; } }
    public float KickChargeTime { get { return _kickChargeTime; } }
    public float MaxKickChargeTime { get { return maxKickChargeTime; } }
    public bool Grabbing { get { return _grabbing; } set { _grabbing = value; } }
    public int GrabPunchAnimation { get { return _grabPunchAnimation; } set { _grabPunchAnimation = value; } }

    public Animator Animator { get { return animator; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStateMachine = player.GetComponent<PlayerStateMachine>();
        playerAim = player.GetComponent<PlayerAim>();
        playerCameraTransform = playerStateMachine.CameraTransform.Find("Camera");

        _states = new HandStateFactory(this);
        CurrentState = _states.Grounded();
        CurrentState.EnterState();
    }

    protected override void Update()
    {
        base.Update();
        HandleKickCharge();
    }

    void HandleKickCharge()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _kickChargeTime += Time.deltaTime;
            _kickChargeTime = Mathf.Clamp(_kickChargeTime, 0, maxKickChargeTime);
        }
    }

    void AddAirPunchImpulse()
    {
        AddPlayerImpulse(airPunchLaunchForce, playerCameraTransform.forward);
    }

    void AddPlayerImpulse(float magnitude, Vector3 direction)
    {
        playerStateMachine.AddImpulse(magnitude, direction);
    }

    void CastPunchOneHit()
    {
        if (playerAim)
        {
            StartCoroutine(playerAim.CastHitRoutine(punchRange, punchBaseDamage, 1, hitActiveFrames));
        }
    }
    void CastPunchTwoHit()
    {
        if (playerAim)
        {
            StartCoroutine(playerAim.CastHitRoutine(punchRange, punchBaseDamage, 2, hitActiveFrames));
        }
    }

    void CastPunchThreeHit()
    {
        if (playerAim)
        {
            StartCoroutine(playerAim.CastHitRoutine(punchRange, punchBaseDamage * 2, 3, hitActiveFrames));
        }
    }

    void CastKickHit()
    {
        if (playerAim)
        {
            StartCoroutine(playerAim.CastKickHitRoutine(kickRange, GetKickLaunchMultiplier(), kickDamage, hitActiveFrames));
            _kickChargeTime = 0;
        }
    }

    void EnablePunch()
    {
        playerStateMachine.CanPunch = true;
    }

    void DisablePunch()
    {
        playerStateMachine.CanPunch = false;
    }

    void LaunchGrabbedEnemy()
    {
        playerAim.LaunchGrabbedEnemy();
    }
    
    void ShakeCamera(float magnitude)
    {
        playerAim.ShakeCamera(magnitude, magnitude * 2);
    }

    void HitStop(float duration)
    {
        playerAim.HitStop(duration);
    }

    void KickLaunchGrabbedEnemy()
    {
        playerAim.KickLaunchGrabbedEnemy(GetKickLaunchMultiplier());
        _kickChargeTime = 0;
    }

    float GetKickLaunchMultiplier()
    {
        float clampedMultiplier = Mathf.Clamp(_kickChargeTime, 0.35f, maxKickChargeTime);
        return clampedMultiplier * 1.5f;
    }

    void SetGrabbingFalse()
    {
        _grabbing = false;
    }

    public void ReleaseGrab()
    {
        playerAim.ReleaseGrabbedEnemy();
    }

    public bool CastGrabHit()
    {
        return playerAim.CastGrabHit(grabRange);
    }

    public int IsTouchingWall()
    {
        return playerStateMachine.IsTouchingWall();
    }

}
