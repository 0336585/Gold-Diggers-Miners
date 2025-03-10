using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private int damage = 1;
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
        if (Time.time >= nextAttackTime)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Check for player hit
        RaycastHit2D playerHit = Physics2D.Raycast(transform.position, direction, attackRange, playerLayer);
        if (playerHit.collider != null && playerHit.collider.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            CharacterStats enemyStats = GetComponent<CharacterStats>();
            CharacterStats playerStats = player.GetComponent<CharacterStats>();

            player.TakeDamage(playerStats, enemyStats);
            nextAttackTime = Time.time + attackRate;
            return; // Exit after attacking the player
        }

        // Check for block hit
        RaycastHit2D blockHit = Physics2D.Raycast(transform.position, direction, attackRange, blockLayer);
        if (blockHit.collider != null && blockHit.collider.TryGetComponent<NodeHealth>(out NodeHealth block))
        {
            block.DamageNode(damage);
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
