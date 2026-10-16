using System.Collections;
using UnityEngine;

public class HandPickupState : HandBaseState
{
    float _animationDuration = 0.5f;

    public HandPickupState(HandStateMachine context, HandStateFactory factory)
    : base(context, factory)
    {
        StateName = "Pickup";
        isRootState = true;
        Debug.Log("Hand entered Pickup state");
    }

    public override void EnterState() 
    {
        _context.Animator.Play("HandPickingUpItem", -1, 0);
        _context.StartCoroutine(InitiateSubStateRoutine());
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {

    }
    public override void ExitState()
    {
        currentSubState?.ExitState();
        _context.PlayerAim.DropCurrentWeapon();
    }
    public override void CheckSwitchStates()
    {
        if (!Input.GetMouseButton(1) && _context.CanDropWeapon)
        {
            SwitchState(_context.Grounded ? _factory.Grounded() : _factory.Airborne());
        }
    }
    public override void InitializeSubState()
    {
        SetSubState(_factory.PickupIdle());
        currentSubState.EnterState();
    }

    IEnumerator InitiateSubStateRoutine()
    {
        yield return new WaitForSeconds(_animationDuration);
        InitializeSubState();
    }
}
