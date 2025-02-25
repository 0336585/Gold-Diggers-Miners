using UnityEngine;

public class ExplosiveDamage : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private bool hasDetonationTime = true;
    [SerializeField] private float detonationTime = 2f;
    [SerializeField] private float explosionRadius = 2f; 

    private float damage; // Damage value passed from ProjectileThrowing

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    private void Update()
    {
        if (hasDetonationTime)
        {
            detonationTime -= Time.deltaTime;

            if (detonationTime <= 0)
            {
                Explode();
                Destroy(gameObject);
            }
        }
    }

    private void Explode()
    {
        // Detect all colliders within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D collider in colliders)
        {
            // Check if the collider has the NoteHealth component
            NodeHealth noteHealth = collider.GetComponent<NodeHealth>();

            if (noteHealth != null)
            {
                // Apply damage to the note
                noteHealth.DamageNode(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the explosion radius in the editor for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
