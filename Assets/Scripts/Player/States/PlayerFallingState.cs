using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerAirState
{
    public PlayerFallingState(BasePlayer _player, StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    private int fallDamageThreshold = 8; // Minimum fall distance to take damage
    private int baseFallDamage = 1; // Base damage for falling
    private float damageMultiplier = 0.5f; // Lower multiplier to reduce harsh fall damage
    private int maxFallDamage = 3; // Prevents instant death from extreme falls

    private float fallStartHeight;
    private bool isFalling;

    private void ApplyFallDamage(int fallDistance)
    {
        // Calculate scaled damage based on fall distance
        int damage = baseFallDamage + Mathf.RoundToInt((fallDistance - fallDamageThreshold) * damageMultiplier);

        // Ensure damage is not negative and does not exceed maxFallDamage
        damage = Mathf.Clamp(damage, baseFallDamage, maxFallDamage);

        //Debug.Log("Fall Distance: " + fallDistance + ", Damage Applied: " + damage);
        player.GetComponent<PlayerHealth>().TakeDamageWithInt(player.GetComponent<CharacterStats>(), damage);
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocityY < 0 && !player.IsGroundDetected())
        {
            if (!isFalling)
            {
                isFalling = true;
                fallStartHeight = player.transform.position.y;
            }
        }
        else
        {
            if (isFalling)
            {
                isFalling = false;
                int fallDistance = Mathf.RoundToInt(fallStartHeight - player.transform.position.y);

                if (fallDistance >= fallDamageThreshold)
                {
                    ApplyFallDamage(fallDistance);
                }
            }
        }
    }
}
