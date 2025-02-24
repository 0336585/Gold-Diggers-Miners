using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float dashSpeed;

    public PlayerDashState(BasePlayer _player, StateMachine _stateMachine, string _animBoolName, float _dashSpeed) : base(_player, _stateMachine, _animBoolName)
    {
        dashSpeed = _dashSpeed;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 0.2f;

        rb.AddForce(new Vector2(player.facingDir * dashSpeed, 0), ForceMode2D.Impulse);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            stateMachine.ChangeState(player.idleState);
    }
}
