using UnityEngine;

public abstract class WeaponProp : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _enemyLaunchForceMultiplier = 1;
    [SerializeField] float _bounceVerticalSpeed = 5;
    [SerializeField] float _bounceHorizontalSpeed = 5;
    [SerializeField] protected float _camShakeMagnitude = 10;
    [SerializeField] protected float _camShakeDuration = 0.01f;
    [SerializeField] protected float _hitStopDuration = 0.05f;
    [SerializeField] protected float _enemyLaunchForce = 200;
    [SerializeField] protected float _objectLaunchForce = 30;
    [SerializeField] protected float _weaponRange = 4;
    [SerializeField] protected int _weaponDamage = 20;
    [SerializeField] protected int _hitActiveFrames = 7;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] LayerMask _objectLayer;


    protected Rigidbody rigidBody;
    protected WeaponActionBase _primaryAction;

    bool grounded = false;
    BoxCollider boxCollider;

    public bool Grounded { get { return grounded; } }
    public WeaponActionBase PrimaryAction { get { return _primaryAction; } }
    public float CamShakeMagnitude { get { return _camShakeMagnitude; } }
    public float CamShakeDuration { get { return _camShakeDuration; } }
    public float HitStopDuration { get { return _hitStopDuration; } }
    public float EnemyLaunchForce { get { return _enemyLaunchForce; } }
    public float ObjectLaunchForce { get { return _objectLaunchForce; } }
    public float WeaponRange { get { return _weaponRange; } }
    public int WeaponDamage { get { return _weaponDamage; } }
    public int HitActiveFrames { get { return _hitActiveFrames; } }

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

    public void EnableInterruptAction()
    {
        _primaryAction.CanInterruptAction = true;
    }

    public virtual bool CastWeaponHit(Ray ray, Vector3 enemyLaunchDirection, Vector3 objectLaunchDirection, PlayerAim playerAim)
    {
        return CastWeaponHit(ray, _weaponRange, _weaponDamage, _enemyLaunchForce, _objectLaunchForce, enemyLaunchDirection, objectLaunchDirection, 
            _camShakeMagnitude, _camShakeDuration, _hitStopDuration, playerAim);
    }

    public virtual bool CastWeaponHit(Ray ray, float range, int damage, float enemyLaunchForce, float objectLaunchForce, 
        Vector3 enemyLaunchDirection, Vector3 objectLaunchDirection, float shakeMagnitude, float shakeDuration, float stopDuration, PlayerAim playerAim)
    {
        Debug.DrawRay(ray.origin, ray.direction * range, Color.blue);

        bool weaponHit = false;

        if (Physics.Raycast(ray, out RaycastHit hit, range, _enemyLayer))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyStateMachine enemyPrototype = hit.transform.GetComponent<EnemyStateMachine>();
                enemyPrototype.TakeDamage(damage);
                enemyPrototype.EnterRagdoll(enemyLaunchForce, 0, enemyLaunchDirection);
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EnemyRagdoll"))
            {
                hit.transform.GetComponent<Rigidbody>()
                    .AddForce(enemyLaunchForce * enemyLaunchDirection, ForceMode.Impulse);
                hit.transform.root.GetComponent<EnemyRagdoll>().TakeHit();
            }
            weaponHit = true;
        }
        else if (Physics.Raycast(ray, out RaycastHit pickHit, range, _objectLayer))
        {
            if (pickHit.transform.GetComponent<Rigidbody>())
            {
                WeaponProp weaponProp = pickHit.transform.GetComponent<WeaponProp>();
                if (weaponProp != null && !weaponProp.Grounded)
                {
                    pickHit.transform.forward = objectLaunchDirection;
                }
                pickHit.transform.GetComponent<Rigidbody>().AddForce(objectLaunchForce * objectLaunchDirection, ForceMode.Impulse);
            }
            weaponHit = true;
        }
        if (weaponHit)
        {
            playerAim.ShakeCamera(shakeMagnitude, shakeDuration);
            playerAim.HitStop(stopDuration);
        }
        return weaponHit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (rigidBody != null && rigidBody.linearVelocity.magnitude > 10)
        {
            if (other.gameObject.tag == "Enemy")
            {
                EnemyStateMachine enemyStateMachine = other.GetComponent<EnemyStateMachine>();
                if (enemyStateMachine != null)
                {
                    Vector3 normalizedVelocity = rigidBody.linearVelocity.normalized;
                    enemyStateMachine.EnterRagdoll(rigidBody.linearVelocity.magnitude * _enemyLaunchForceMultiplier, 0, normalizedVelocity);
                    rigidBody.angularVelocity = Vector3.zero;
                    rigidBody.linearVelocity = (new Vector3(-normalizedVelocity.x, 0, -normalizedVelocity.z).normalized * _bounceHorizontalSpeed)
                        + Vector3.up * _bounceVerticalSpeed;
                    transform.forward = other.transform.forward;

                    rigidBody.AddTorque(-transform.forward * rigidBody.linearVelocity.magnitude / 2);
                }
            }
            else if (other.gameObject.tag == "EnemyRagdoll" && other.GetComponent<Rigidbody>() != null)
            {
                Vector3 normalizedVelocity = rigidBody.linearVelocity.normalized;
                other.GetComponent<Rigidbody>().AddForce(normalizedVelocity * rigidBody.linearVelocity.magnitude * _enemyLaunchForceMultiplier, ForceMode.Impulse);
                rigidBody.angularVelocity = Vector3.zero;
                rigidBody.linearVelocity = (new Vector3(-normalizedVelocity.x, 0, -normalizedVelocity.z).normalized * _bounceHorizontalSpeed)
                    + Vector3.up * _bounceVerticalSpeed;
                transform.forward = other.transform.forward;

                rigidBody.AddTorque(-transform.forward * rigidBody.linearVelocity.magnitude / 2);
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
