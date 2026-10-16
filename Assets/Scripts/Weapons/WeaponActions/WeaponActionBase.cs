
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
    protected bool _canExecuteAction = true;
    protected bool _endAction = false;
    protected float _actionDuration;
    /** 
     * _canInterruptAction is used to cancel this action (usually into itself to animation cancel), this might be set to true 
     * via animation events, check HandsContainer animations
     */
    protected bool _canInterruptAction = false;

    bool _executingAction;
    
    public bool ExecutingAction { get {  return _executingAction; } set { _executingAction = value; } }
    public bool EndAction { get { return _endAction; } }
    public float ActionDuration { get { return _actionDuration; } set { _actionDuration = value; } }
    public float ActionInitialDuration { get { return _actionInitialDuration; } }
    public bool CanInterruptAction { get { return _canInterruptAction; } set { _canInterruptAction = value; } }
    public abstract void Action(HandStateMachine handStateMachine);
    public virtual void ResetAction()
    {
        _endAction = false;
        _actionDuration = _actionInitialDuration;
        _canExecuteAction = true;
        _canInterruptAction = false;
    }
}
