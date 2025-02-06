using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    private float jumpForce;

    public PlayerJumpState(BasePlayer _player, StateMachine _stateMachine, string _animBoolName, float _jumpForce) : base(_player, _stateMachine, _animBoolName)
    {
        jumpForce = _jumpForce;
    }

    public override void Enter()
    {
        base.Enter();

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.fallingState);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
