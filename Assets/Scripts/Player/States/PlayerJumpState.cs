using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    private float jumpForce;

    public PlayerJumpState(BasePlayer _player, StateMachine _stateMachine, string _animBoolName, float _jumpForce) : base(_player, _stateMachine, _animBoolName)
    {
        jumpForce = _jumpForce;
    }

    public override void Enter()
    {
        base.Enter();

        
    }

    public override void Update()
    {
        base.Update();

        for (int i = 0; i < 50; i++)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            if (rb.linearVelocityY > 5)
                break;
        }

        if (rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.fallingState);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
