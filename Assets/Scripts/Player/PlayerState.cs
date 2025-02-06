using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    protected BasePlayer player;
    protected StateMachine stateMachine;

    protected float xInput;
    protected float yInput;

    public PlayerState(BasePlayer _player, StateMachine _stateMachine, string _animBoolName)
    {
        player = _player;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;

        rb = player.rb;
        animator = player.animator;
    }

    public override void Update()
    {
        base.Update();

        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        if (!player.IsGroundDetected())
        {
            if (stateMachine.currentState == player.jumpState) return;
            if (stateMachine.currentState == player.dashState) return;

            stateMachine.ChangeState(player.fallingState);
        }
    }
}
