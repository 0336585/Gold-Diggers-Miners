using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{

    private float jumpCooldown = 0.2f; // Delay between jumps
    private float lastJumpTime = 0f;  // Time of the last jump

    public PlayerGroundedState(BasePlayer _player, StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.Space) && Time.time > lastJumpTime + jumpCooldown)
        {
            stateMachine.ChangeState(player.jumpState);
            lastJumpTime = Time.time; // Update last jump time
        }
    }
}
