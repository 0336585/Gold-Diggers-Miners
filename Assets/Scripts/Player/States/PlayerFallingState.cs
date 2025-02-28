using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerAirState
{
    public PlayerFallingState(BasePlayer _player, StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public float fallDamageThreshold = 5f; // Minimum fall distance to take damage
    public int baseFallDamage = 1; // Base damage for falling
    public float damageMultiplier = 2f; // Multiplier for scaling damage based on fall distance

    private float fallStartHeight;
    private bool isFalling;

    private void ApplyFallDamage(float fallDistance)
    {
        // Calculate scaled damage based on fall distance
        int damage = Mathf.RoundToInt(baseFallDamage + (fallDistance - fallDamageThreshold) * damageMultiplier);

        // Ensure damage is not negative
        damage = Mathf.Max(damage, baseFallDamage);

        Debug.Log("Fall Distance: " + fallDistance + ", Damage Applied: " + damage);
        player.GetComponent<PlayerHealth>().TakeDamageWithFloat(player.GetComponent<CharacterStats>(), damage);
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
                float fallDistance = fallStartHeight - player.transform.position.y;

                if (fallDistance >= fallDamageThreshold)
                {
                    ApplyFallDamage(fallDistance);
                }
            }
        }
    }
}
