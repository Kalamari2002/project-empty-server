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
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            parentRagdoll.grounded = false;
        }
    }
}
