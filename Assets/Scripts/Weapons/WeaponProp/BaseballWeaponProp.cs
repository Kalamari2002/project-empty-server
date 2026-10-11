using UnityEngine;

public class BaseballWeaponProp : WeaponProp
{
    protected override void Awake()
    {
        base.Awake();
        _primaryAction = new BaseballWeaponAction();
    }
}
