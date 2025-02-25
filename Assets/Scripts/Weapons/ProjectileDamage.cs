using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    private float damageAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
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