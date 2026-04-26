using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float punchResetTime = .5f;
    [SerializeField] float punchRange = 5;
    [SerializeField] int punchBaseDamage = 10;

    [Header("Debugging")]
    [SerializeField] int currentPunch = 0;
    [SerializeField] float punchCountDown;
    [SerializeField] bool punching = false;
    [SerializeField] bool canPunch = true;

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
        if (Input.GetMouseButtonDown(0))
        {
            HandlePunch();
        }
    }

    void HandlePunch()
    {
        if (!canPunch) return;
        canPunch = false;
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

    void CastHit()
    {
        if (playerAim)
        {
            playerAim.CastHit(punchRange, currentPunch == 3 ? punchBaseDamage * 2 : punchBaseDamage, currentPunch);
        }
    }

    void TriggerPlayerImpulse(float impulse)
    {
        playerAim.Impulse(impulse);
    }
}
