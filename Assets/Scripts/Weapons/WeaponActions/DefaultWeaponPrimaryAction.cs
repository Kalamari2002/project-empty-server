
public class DefaultWeaponPrimaryAction : WeaponActionBase
{
    int animationCounter = 1;
    public DefaultWeaponPrimaryAction()
    {
        _actionInitialDuration = 1;
    }
    public override void Action(HandStateMachine handStateMachine)
    {
        handStateMachine.Animator.Play("HandPickupDefaultHit" + animationCounter.ToString());
    }
}
