using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float punchResetTime = .5f;
    [Header("Debugging")]
    [SerializeField] int currentPunch = 0;
    [SerializeField] float punchCountDown;
    [SerializeField] bool punching = false;
    [SerializeField] bool canPunch = true;

    Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
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
        currentPunch = currentPunch == 2 ? 1 : currentPunch + 1;
        punching = true;
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
}
