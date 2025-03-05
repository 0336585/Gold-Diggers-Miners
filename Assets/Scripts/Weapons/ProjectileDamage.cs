using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    private float damageAmount;
    private float spawnTime;
    [SerializeField] private float nonCollisionSeconds = 0.001f;

    private void Start()
    {
        spawnTime = Time.time; // Store the time when the projectile was spawned
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collisions for the first seconds
        if (Time.time - spawnTime < nonCollisionSeconds)
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
