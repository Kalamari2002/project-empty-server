using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    [Header("Punch Settings")]
    [SerializeField] float punchRange = 5;
    [SerializeField] int punchBaseDamage = 10;

    [Header("Kick Settings")]
    [SerializeField] float kickRange = 4;
    [SerializeField] int kickDamage = 5;

    [Header("Debugging")]
    [SerializeField] int currentPunch = 0;
    [SerializeField] bool punching = false;
    [SerializeField] bool canPunch = true;
    [SerializeField] bool kicking = false;
    [SerializeField] bool canKick = true;

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
    }

    void HandlePunch()
    {
        if (!canPunch || kicking) return;
        canPunch = false;
        canKick = false;
        currentPunch = currentPunch == 3 ? 1 : currentPunch + 1;
        punching = true;
        animator.speed = currentPunch == 1 ? 1.5f : 1;
        animator.SetBool("Punching", punching);
        animator.SetInteger("CurrentPunch", currentPunch);
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

    void HandleKick()
    {
        if (!canKick || kicking) return;
        canPunch = false;
        canKick = false;
        kicking = true;
        animator.SetBool("Kicking", kicking);
        animator.Play("Kick", -1, 0);
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

    void CastKickHit()
    {
        if (playerAim)
        {
            playerAim.CastKickHit(kickRange, kickDamage);
        }
    }

    void TriggerPlayerImpulse(float impulse)
    {
        playerAim.Impulse(impulse);
    }
}
