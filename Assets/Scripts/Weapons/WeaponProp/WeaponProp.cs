using UnityEngine;

public abstract class WeaponProp : MonoBehaviour
{
    [SerializeField] protected GameObject weaponMesh;
    [SerializeField] protected Rigidbody rigidBody;

    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public virtual WeaponBase PickUp(Transform parent)
    {
        GameObject mesh = Instantiate(weaponMesh, parent.transform.position, Quaternion.identity);
        mesh.transform.SetParent(parent);
        mesh.transform.localPosition = Vector3.zero;
        mesh.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        Destroy(rigidBody);
        return InstantiateWeapon(gameObject);
    }

    protected abstract WeaponBase InstantiateWeapon(GameObject weaponProp);
}
