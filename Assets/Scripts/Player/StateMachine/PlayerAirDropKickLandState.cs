using UnityEngine;

public class PlayerAirDropKickLandState : PlayerBaseState
{
    float _transitionDuration = 0.4f;

    public PlayerAirDropKickLandState(PlayerStateMachine context, PlayerStateFactory factory)
    : base(context, factory)
    {
        name = "AirMoveDropKickLand";
        InitializeSubState();
    }

    public override void EnterState()
    {
        _context.OrientationAnimator.SetBool("Grounded", true);
    }
    public override void UpdateState()
    {
        _transitionDuration -= Time.deltaTime;
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
    }
    public override void ExitState() 
    {
        _context.DropKicking = false;
    }
    public override void CheckSwitchStates()
    {
        if (_transitionDuration <= 0)
        {
            SwitchState(_factory.Move());
        }
    }
    public override void InitializeSubState()
    {

    }
}
