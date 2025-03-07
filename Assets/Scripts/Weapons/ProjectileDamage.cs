using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    private float damageAmount;
    private float spawnTime;
    private ProjectileShooter shooter;  

    private void Start()
    {
        spawnTime = Time.time; // Store the time when the projectile was spawned
    }

    public void SetShooter(ProjectileShooter shooter)
    {
        this.shooter = shooter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collisions for the first milisecond
        if (Time.time - spawnTime < 0.001f)
            return;

        // Check if the colliding object has the NodeHealth component
        NodeHealth nodeHealth = collision.GetComponent<NodeHealth>();
        BaseHealth baseHealth = collision.GetComponent<BaseHealth>();

        // Check if the collision is with another projectile from the same shooter
        if (collision.TryGetComponent<ProjectileDamage>(out var otherProjectileDamage))
        {
            // Ignore the collision if the other projectile was fired by the same shooter
            if (otherProjectileDamage.shooter == shooter)
            {
                return;
            }
        }

        if (nodeHealth != null)
        {
            nodeHealth.DamageNode(damageAmount);
        }

        if (baseHealth != null)
        {
            CharacterStats characterStats = collision.GetComponent<CharacterStats>();
            baseHealth.TakeDamageWithInt(characterStats, (int)damageAmount);
        }

        // Destroy the projectile after hitting something
        Destroy(gameObject);
    }

    public void SetDamage(float damage)
    {
        damageAmount = damage;
    }

    public float GetDamage()
    {
        return damageAmount;
    }
}
