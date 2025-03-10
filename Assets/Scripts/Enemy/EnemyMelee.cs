using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private LayerMask playerLayer;

    private EnemyTargeting enemyTargeting;
    private float nextAttackTime = 0f;

    private void Start()
    {
        enemyTargeting = GetComponent<EnemyTargeting>();
        nextAttackTime = attackRate;
    }

    private void Update()
    {
        if (enemyTargeting.TargetPlayer != null && Time.time >= nextAttackTime)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, playerLayer);

        if (hit.collider != null && hit.collider.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            // Assuming the enemy has a CharacterStats component
            CharacterStats enemyStats = GetComponent<CharacterStats>();
            // Assuming the player GameObject has a CharacterStats component too
            CharacterStats playerStats = player.GetComponent<CharacterStats>();

            player.TakeDamage(playerStats, enemyStats);
            nextAttackTime = Time.time + attackRate;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * attackRange);
    }
}
