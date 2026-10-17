using UnityEngine;

public class PickableAction : WeaponActionBase
{
    public override void Action(HandStateMachine handStateMachine)
    {
        // This is no-op for now, not sure what i want to do for the primary action of generic pickable objects
        Debug.Log("Pickable Actioned!");
        _endAction = true;
    }
}
