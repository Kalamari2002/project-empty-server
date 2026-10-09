using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected GameObject weaponProp;

    public GameObject WeaponProp { get { return weaponProp; } }

    public WeaponBase(GameObject prefab)
    {
        weaponProp = prefab;
    }

    public virtual void Drop(Vector3 dropPosition, Vector3 launchDirection, Vector3 torque)
    {
        GameObject prop = Instantiate(weaponProp, dropPosition, Quaternion.identity);
        Rigidbody propRigidBody = prop.GetComponent<Rigidbody>();
        propRigidBody.AddForce(launchDirection, ForceMode.Impulse);
        propRigidBody.AddTorque(torque);
    }

    public abstract void PrimaryAction(Collider collider);
}
