using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerAirState
{
    public PlayerFallingState(BasePlayer _player, StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (xInput < 0 && player.facingRight)
            player.Flip();
        else if (xInput > 0 && !player.facingRight)
            player.Flip();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        //player.baseMovement.SetVelocity(xInput, rb.linearVelocity.y, rb);
    }
}
