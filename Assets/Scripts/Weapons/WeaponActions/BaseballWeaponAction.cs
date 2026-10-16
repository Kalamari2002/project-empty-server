using UnityEngine;

public class BaseballWeaponAction : WeaponActionBase
{
    const float LIGHT_HIT_THRESHOLD = 0.2f;
    const float HEAVY_HIT_ANIMATION_DURATION = 0.783f;
    const float LIGHT_HIT_ANIMATION_DURATION = 0.816f;
    float _chargeTime = 0;
    BaseballHitType _hitType = BaseballHitType.NONE;

    public BaseballHitType HitType { get { return _hitType; } }

    public enum BaseballHitType
    {
        NONE,
        LIGHT,
        HEAVY
    }

    public BaseballWeaponAction() : base()
    {
        _actionInitialDuration = HEAVY_HIT_ANIMATION_DURATION;
        _actionDuration = _actionInitialDuration;
    }

    public override void Action(HandStateMachine handStateMachine)
    {
        if (_canExecuteAction)
        {
            _chargeTime += Time.deltaTime;
            handStateMachine.Animator.Play("HandPickupBaseballCharge");
            if (Input.GetMouseButtonUp(0))
            {
                handStateMachine.Animator.Play(_chargeTime <= LIGHT_HIT_THRESHOLD ? "HandPickupBaseballLightHit" : "HandPickupBaseballHeavyHit", -1, 0);
                _actionDuration = _chargeTime <= LIGHT_HIT_THRESHOLD ? LIGHT_HIT_ANIMATION_DURATION : HEAVY_HIT_ANIMATION_DURATION;
                _hitType = _chargeTime <= LIGHT_HIT_THRESHOLD ? BaseballHitType.LIGHT : BaseballHitType.HEAVY;
                _canExecuteAction = false;
            }
        }
        else
        {
            _actionDuration -= Time.deltaTime;
            if (_actionDuration <= 0)
            {
                _endAction = true;
            }
        }
    }

    public override void ResetAction()
    {
        base.ResetAction();
        _chargeTime = 0;
        _hitType = BaseballHitType.NONE;
    }
}
