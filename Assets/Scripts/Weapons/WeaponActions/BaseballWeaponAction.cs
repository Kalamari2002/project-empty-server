using UnityEngine;

public class BaseballWeaponAction : WeaponActionBase
{
    public BaseballWeaponAction()
    {
        _actionInitialDuration = 1.366f;
    }
    public override void Action(HandStateMachine handStateMachine)
    {
        handStateMachine.Animator.Play("HandPickupBaseballHit");
    }
}
