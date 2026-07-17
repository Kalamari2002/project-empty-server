using UnityEngine;

public class PlayerHands : MonoBehaviour
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

    [Header("Debugging")]
    [SerializeField] int currentPunch = 0;
    [SerializeField] int pummel = 0;
    [SerializeField] bool punching = false;
    [SerializeField] bool canPunch = true;
    [SerializeField] bool kicking = false;
    [SerializeField] bool canKick = true;
    [SerializeField] bool grabbing = false;

    PlayerAim playerAim;
    Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject Player = GameObject.Find("Player");
        if (Player)
        {
            Debug.Log("Testt");
            playerAim = Player.GetComponent<PlayerAim>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            HandleKick();
        }
        if (Input.GetMouseButtonDown(0))
        {
            HandlePunch();
        }
        if (Input.GetMouseButtonDown(1))
        {
            HandleGrab();
        }
        HandleRelease();
    }

    void HandlePunch()
    {
        if (!canPunch || kicking) return; 
        canPunch = false;
        canKick = false;
        punching = true;
        animator.SetBool("Punching", punching);
        if (!grabbing)
        {
            currentPunch = currentPunch == 3 ? 1 : currentPunch + 1;
            animator.speed = currentPunch == 1 ? 1.5f : 1;
            animator.SetInteger("CurrentPunch", currentPunch);
        }
        else
        {
            pummel++;
            if (pummel == 3)
            {
                animator.Play("HandGrabPunchFinal", -1, 0);
                ResetGrab();
            }
            else
            {
                animator.Play("HandGrabPunch", -1, 0);
            }
        }
    }

    void HandleGrab()
    {
        if (grabbing) return;
        grabbing = playerAim.CastGrabHit(grabRange);
        if (grabbing)
        {
            animator.Play("HandGrabIdle", -1, 0);
        }
    }

    void HandleRelease()
    {
        if (!grabbing || punching || kicking || Input.GetMouseButton(1)) return;
        playerAim.ReleaseGrabbedEnemy();
        animator.Play("HandIdle", -1, 0);
        ResetGrab();
    }

    void EnablePunching()
    {
        canPunch = true;
    }

    void ResetPunch()
    {
        canPunch = true;
        currentPunch = 0;
        punching = false;
        animator.SetBool("Punching", punching);
        animator.SetInteger("CurrentPunch", currentPunch);
    }

    void ResetGrab()
    {
        grabbing = false;
        pummel = 0;
    }

    void HandleKick()
    {
        if (!canKick || kicking) return;
        canPunch = false;
        canKick = false;
        kicking = true;
        animator.SetBool("Kicking", kicking);
        animator.Play(grabbing ? "HandGrabKick" : "Kick", -1, 0);
        ResetGrab();
    }

    void EnableKicking()
    {
        canKick = true;
    }

    void ResetKick()
    {
        canKick = true;
        kicking = false;
        animator.SetBool("Kicking", kicking);
    }

    void CastHit()
    {
        if (playerAim)
        {
            playerAim.CastHit(punchRange, currentPunch == 3 ? punchBaseDamage * 2 : punchBaseDamage, currentPunch);
        }
    }

    //void CastKickHit()
    //{
    //    if (playerAim)
    //    {
    //        playerAim.CastKickHit(kickRange, kickDamage);
    //    }
    //}

    void TriggerPlayerImpulse(float impulse)
    {
        playerAim.Impulse(impulse);
    }

    //void LaunchGrabbedEnemy()
    //{
    //    playerAim.LaunchGrabbedEnemy();
    //}

    //void KickLaunchGrabbedEnemy()
    //{
    //    playerAim.KickLaunchGrabbedEnemy();
    //}

    //void ShakeCamera(float magnitude)
    //{
    //    playerAim.ShakeCamera(magnitude, magnitude * 2);
    //}

    //void HitStop(float duration)
    //{
    //    playerAim.HitStop(duration);
    //}
}
