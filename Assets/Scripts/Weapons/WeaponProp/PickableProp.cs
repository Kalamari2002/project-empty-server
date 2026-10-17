using UnityEngine;

public class PickableProp : WeaponProp
{
    protected override void Awake()
    {
        base.Awake();
        _primaryAction = new PickableAction();
    }
}
