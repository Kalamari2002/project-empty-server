using UnityEngine;

public abstract class WeaponProp : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float enemyLaunchForceMultiplier = 1;
    [SerializeField] float bounceVerticalSpeed = 5;
    [SerializeField] float bounceHorizontalSpeed = 5;
    [SerializeField] protected float camShakeMagnitude = 10;
    [SerializeField] protected float camShakeDuration = 0.01f;
    [SerializeField] protected float hitStopDuration = 0.05f;
    [SerializeField] protected float enemyLaunchForce = 200;
    [SerializeField] protected float objectLaunchForce = 30;
    [SerializeField] protected float weaponRange = 4;
    [SerializeField] protected int weaponDamage = 20;
    [SerializeField] protected int hitActiveFrames = 7;


    protected Rigidbody rigidBody;
    protected WeaponActionBase _primaryAction;

    bool grounded = false;
    BoxCollider boxCollider;

    public bool Grounded { get { return grounded; } }
    public WeaponActionBase PrimaryAction { get { return _primaryAction; } }
    public float CamShakeMagnitude { get { return camShakeMagnitude; } }
    public float CamShakeDuration { get { return camShakeDuration; } }
    public float HitStopDuration { get { return hitStopDuration; } }
    public float EnemyLaunchForce { get { return enemyLaunchForce; } }
    public float ObjectLaunchForce { get { return objectLaunchForce; } }
    public float WeaponRange { get { return weaponRange; } }
    public int WeaponDamage { get { return weaponDamage; } }
    public int HitActiveFrames { get { return hitActiveFrames; } }

    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        _primaryAction = new DefaultWeaponPrimaryAction();
    }

    public virtual WeaponProp PickUp(Transform parent)
    {
        Destroy(rigidBody);
        foreach(Collider collider in transform.Find("Mesh").GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        boxCollider.enabled = false;
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        grounded = false;
        return this;
    }

    public virtual void Drop(Vector3 dropPosition, Vector3 launchDirection, Vector3 torque)
    {
        Drop(dropPosition, launchDirection, launchDirection, torque);
    }

    public virtual void Drop(Vector3 dropPosition, Vector3 orientation, Vector3 launchDirection, Vector3 torque)
    {
        transform.SetParent(null);
        transform.position = dropPosition;
        transform.up = orientation;
        rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
        rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        boxCollider.enabled = true;
        foreach (Collider collider in transform.Find("Mesh").GetComponentsInChildren<Collider>())
        {
            collider.enabled = true;
        }
        rigidBody.AddForce(launchDirection, ForceMode.Impulse);
        rigidBody.AddTorque(torque);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && rigidBody != null && rigidBody.linearVelocity.magnitude > 10)
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

        if (other.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }
}
