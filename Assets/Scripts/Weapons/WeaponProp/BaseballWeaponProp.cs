using UnityEngine;

public class BaseballWeaponProp : WeaponProp
{
    [SerializeField] float _lightHitLaunchForce = 100;
    [SerializeField] float _lightObjectHitLaunchForce = 5;
    [SerializeField] int _lightHitDamage = 10;

    protected override void Awake()
    {
        base.Awake();
        _primaryAction = new BaseballWeaponAction();
    }

    public override bool CastWeaponHit(Ray ray, Vector3 enemyLaunchDirection, Vector3 objectLaunchDirection, PlayerAim playerAim)
    {
        BaseballWeaponAction baseballWeaponAction = (BaseballWeaponAction)_primaryAction;
        if (baseballWeaponAction.HitType == BaseballWeaponAction.BaseballHitType.LIGHT)
        {
            return CastWeaponHit(ray, _weaponRange, _lightHitDamage, _lightHitLaunchForce, _lightObjectHitLaunchForce, Vector3.up, Vector3.up, 
                _camShakeMagnitude / 3, _camShakeMagnitude / 3, _hitStopDuration / 3, playerAim);
        }
        return base.CastWeaponHit(ray, _weaponRange, _weaponDamage, _enemyLaunchForce, _objectLaunchForce, enemyLaunchDirection, objectLaunchDirection,
            _camShakeMagnitude, _camShakeDuration, _hitStopDuration, playerAim);
    }
}
