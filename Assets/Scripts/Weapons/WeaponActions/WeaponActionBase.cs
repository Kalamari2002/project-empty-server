
using UnityEngine;

/**
 * Because this class does not inherit from Monobehavior, it doesn't have an Update method
 * so an Action's attributes (_executingAction and _actionDuration) is controlled by the 
 * handStateMachine that holds the current WeaponProp this Action belongs to.
 */
public abstract class WeaponActionBase
{
    protected float _actionInitialDuration;
    protected float _range;
    protected int _damage;


    bool _executingAction;
    float _actionDuration;
    
    public bool ExecutingAction { get {  return _executingAction; } set { _executingAction = value; } }
    public float ActionDuration { get { return _actionDuration; } set { _actionDuration = value; } }
    public float ActionInitialDuration { get { return _actionInitialDuration; } }
    public abstract void Action(HandStateMachine handStateMachine);
}
