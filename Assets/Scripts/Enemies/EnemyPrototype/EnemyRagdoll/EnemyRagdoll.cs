using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float ragdollDuration;
    public bool grounded;
    [Header("Debugging")]
    [SerializeField] float ragdollCountDown;
    EnemyPrototype parentEnemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ragdollCountDown = ragdollDuration;
    }

    public void SetUp(EnemyPrototype parentEnemy)
    {
        this.parentEnemy = parentEnemy;
    }

    // Update is called once per frame
    void Update()
    {
        if (grounded)
        {
            ragdollCountDown -= Time.deltaTime;
        }
        if (ragdollCountDown <= 0 && parentEnemy != null)
        {
            parentEnemy.WakeUpFromRagdoll();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            grounded = false;
        }
    }

    public void TakeHit()
    {
        ragdollCountDown = ragdollDuration;
    }

    public EnemyPrototype GetParentEnemy()
    {
        return parentEnemy;
    }
}
