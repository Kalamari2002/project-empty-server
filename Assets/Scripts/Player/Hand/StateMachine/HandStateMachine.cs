using UnityEngine;

public class HandStateMachine : BaseStateMachine
{
    [Header("Punch Settings")]
    [SerializeField] float punchRange = 5;
    [SerializeField] int punchBaseDamage = 10;

    [Header("Kick Settings")]
    [SerializeField] float kickRange = 4;
    [SerializeField] int kickDamage = 5;

    [Header("Grab Settings")]
    [SerializeField] float grabRange = 5;

    [Header("Feedback Settings")]
    [SerializeField] float camShakeMagnitude = 0.1f;
    [SerializeField] float camShakeDuration = 0.2f;
    [SerializeField] float hitStopDuration = 0.05f;

    Animator animator;
    PlayerAim playerAim;

    PlayerStateMachine playerStateMachine;
    HandStateFactory _states;

    bool _canPunch = true;

    public bool Grounded { get { return playerStateMachine.Grounded; } }
    public bool CanPunch { get { return _canPunch; } set { _canPunch = value; } }

    public Animator Animator { get { return animator; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();

        _states = new HandStateFactory(this);
        CurrentState = _states.Grounded();
        CurrentState.EnterState();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStateMachine = player.GetComponent<PlayerStateMachine>();
        playerAim = player.GetComponent<PlayerAim>();
    }
    void CastPunchOneHit()
    {
        if (playerAim)
        {
            playerAim.CastHit(punchRange, punchBaseDamage, 1);
        }
        else
        {
            Debug.LogError("playerAim NOT FOUND");
        }
    }
    void CastPunchTwoHit()
    {
        if (playerAim)
        {
            playerAim.CastHit(punchRange, punchBaseDamage, 2);
        }
    }

    void CastPunchThreeHit()
    {
        if (playerAim)
        {
            playerAim.CastHit(punchRange, punchBaseDamage * 2, 3);
        }
    }

    void CastKickHit()
    {
        if (playerAim)
        {
            playerAim.CastKickHit(kickRange, kickDamage);
        }
    }

    void EnablePunch()
    {
        _canPunch = true;
    }

    void DisablePunch()
    {
        _canPunch = false;
    }

    public bool CastGrabHit()
    {
        return playerAim.CastGrabHit(grabRange);
    }

    void LaunchGrabbedEnemy()
    {
        playerAim.LaunchGrabbedEnemy();
    }

    public void ReleaseGrab()
    {
        playerAim.ReleaseGrabbedEnemy();
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
        playerAim.KickLaunchGrabbedEnemy();
    }

}
