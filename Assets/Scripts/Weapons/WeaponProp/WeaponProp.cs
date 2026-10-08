using UnityEngine;

public abstract class WeaponProp : MonoBehaviour
{
    [SerializeField] protected GameObject weaponMesh;
    [SerializeField] protected Rigidbody rigidBody;

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

    protected abstract WeaponBase InstantiateWeapon(GameObject weaponProp);
}
