using UnityEngine;

public class EnemyRagdollCollision : MonoBehaviour
{
    [SerializeField] EnemyRagdoll parentRagdoll;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (parentRagdoll == null)
        {
            Debug.Log("EnemyRagdollCollision Error: " + transform.name + " does not have a parentRagdoll assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            parentRagdoll.grounded = true;
        }

        if (collision.gameObject.tag == "Enemy" && parentRagdoll.RigidBody != null && parentRagdoll.RigidBody.linearVelocity.magnitude > 1)
        {
            EnemyStateMachine enemyStateMachine = collision.transform.GetComponent<EnemyStateMachine>();
            if (enemyStateMachine != null)
            {
                Vector3 normalizedVelocity = parentRagdoll.RigidBody.linearVelocity.normalized;
                float nudge = 5f;
                enemyStateMachine.EnterRagdoll(parentRagdoll.RigidBody.linearVelocity.magnitude * nudge, 0, normalizedVelocity);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            parentRagdoll.grounded = false;
        }
    }
}
