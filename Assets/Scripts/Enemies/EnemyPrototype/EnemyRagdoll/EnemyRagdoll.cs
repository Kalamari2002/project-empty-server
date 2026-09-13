using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float ragdollDuration;
    public bool grounded;
    [SerializeField] EnemyStateMachine parentEnemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void SetUp(EnemyStateMachine parentEnemy)
    {
        this.parentEnemy = parentEnemy;
        parentEnemy.ResetRagdollCountDown();
    }

    // Update is called once per frame
    void Update()
    {
        if (grounded)
        {
            parentEnemy.RagdollCountDown -= Time.deltaTime;
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
        parentEnemy.RagdollCountDown = ragdollDuration;
    }

    public EnemyStateMachine GetParentEnemy()
    {
        return parentEnemy;
    }
}
