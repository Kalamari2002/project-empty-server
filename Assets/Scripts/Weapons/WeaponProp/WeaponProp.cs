using UnityEngine;

public abstract class WeaponProp : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float enemyLaunchForceMultiplier = 1;
    [SerializeField] float bounceVerticalSpeed = 5;
    [SerializeField] float bounceHorizontalSpeed = 5;

    protected Rigidbody rigidBody;

    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public virtual WeaponProp PickUp(Transform parent)
    {
        Destroy(rigidBody);
        foreach(Collider collider in transform.Find("Mesh").GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        return this;
    }

    public virtual void Drop(Vector3 dropPosition, Vector3 launchDirection, Vector3 torque)
    {
        transform.SetParent(null);
        transform.position = dropPosition;
        transform.up = launchDirection;
        rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
        rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        foreach (Collider collider in transform.Find("Mesh").GetComponentsInChildren<Collider>())
        {
            collider.enabled = true;
        }
        rigidBody.AddForce(launchDirection, ForceMode.Impulse);
        rigidBody.AddTorque(torque);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && rigidBody != null && rigidBody.linearVelocity.magnitude > 5)
        {
            EnemyStateMachine enemyStateMachine = other.GetComponent<EnemyStateMachine>();
            if (enemyStateMachine != null) 
            {
                Vector3 normalizedVelocity = rigidBody.linearVelocity.normalized;
                enemyStateMachine.EnterRagdoll(rigidBody.linearVelocity.magnitude * enemyLaunchForceMultiplier, 0, normalizedVelocity);
                rigidBody.angularVelocity = Vector3.zero;
                rigidBody.linearVelocity = (new Vector3(-normalizedVelocity.x, 0, -normalizedVelocity.z).normalized * bounceHorizontalSpeed) 
                    + Vector3.up * bounceVerticalSpeed;
                transform.forward = other.transform.forward;

                rigidBody.AddTorque(-transform.forward * rigidBody.linearVelocity.magnitude/2);
            }
        }
    }
}
