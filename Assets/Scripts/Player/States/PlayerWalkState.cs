using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerGroundedState
{
    private BaseMovement baseMovement;

    public PlayerWalkState(BasePlayer _player, StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        baseMovement = player.baseMovement;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (xInput == 0)
            stateMachine.ChangeState(player.idleState);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        baseMovement.SetVelocityX(xInput);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
