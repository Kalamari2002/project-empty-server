using UnityEngine;

public class BaseballWeaponProp : WeaponProp
{
    protected override WeaponBase InstantiateWeapon(GameObject weaponProp)
    {
        return new BaseballWeapon(weaponProp);
    }
}
