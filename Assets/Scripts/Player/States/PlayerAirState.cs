using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private BaseMovement baseMovement;

    public PlayerAirState(BasePlayer _player, StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        baseMovement = player.baseMovement;
    }

    public override void Update()
    {
        base.Update();


        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        if (xInput < 0 && player.facingRight)
            player.Flip();
        else if (xInput > 0 && !player.facingRight)
            player.Flip();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        baseMovement.SetVelocityX(xInput);
    }
}
