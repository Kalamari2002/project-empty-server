using UnityEngine;

public class BaseballWeapon : WeaponBase
{
    public BaseballWeapon(GameObject prefab) : base(prefab) { }

    public override void PrimaryAction(Collider collider)
    {
        Debug.Log("Baseball Swing");
    }
}
