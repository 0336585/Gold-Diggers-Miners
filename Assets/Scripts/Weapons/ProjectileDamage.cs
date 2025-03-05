using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    private float damageAmount;
    private float spawnTime;

    private void Start()
    {
        spawnTime = Time.time; // Store the time when the projectile was spawned
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collisions for the first millisecond (0.001s)
        if (Time.time - spawnTime < 1f)
            return;

        // Check if the colliding object has the NodeHealth component
        NodeHealth nodeHealth = collision.GetComponent<NodeHealth>();

        if (nodeHealth != null)
        {
            nodeHealth.DamageNode(damageAmount);
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
