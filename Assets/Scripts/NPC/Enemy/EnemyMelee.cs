using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float blockDamage = 0.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask blockLayer;  

    private EnemyTargeting enemyTargeting;
    private float nextAttackTime = 0f;

    private void Start()
    {
        enemyTargeting = GetComponent<EnemyTargeting>();
        nextAttackTime = attackRate;
    }

    private void Update()
    {
        if (Time.time >= nextAttackTime && enemyTargeting.TargetPlayer != null)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Determine the attack direction based on the enemy's facing; if facing right (positive x scale), attack to the right; if facing left (negative x scale), attack to the left.
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Perform a raycast to detect any collider in the path, considering both player and block layers.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, playerLayer | blockLayer);
        if (hit.collider != null)
        {
            // Check if the raycast hit a collider on the player layer.
            if (((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
            {
                // The raycast hit the player
                if (hit.collider.TryGetComponent<PlayerHealth>(out PlayerHealth player))
                {
                    CharacterStats enemyStats = GetComponent<CharacterStats>();
                    CharacterStats playerStats = player.GetComponent<CharacterStats>();

                    player.TakeDamage(playerStats, enemyStats);
                    nextAttackTime = Time.time + attackRate;
                }
            }
            else if (((1 << hit.collider.gameObject.layer) & blockLayer) != 0)
            {
                // The raycast hit a block
                if (hit.collider.TryGetComponent<NodeHealth>(out NodeHealth block))
                {
                    block.DamageNode(blockDamage);
                    nextAttackTime = Time.time + attackRate;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * attackRange);
    }
}
