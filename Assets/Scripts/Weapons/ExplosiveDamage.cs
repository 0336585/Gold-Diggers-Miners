using UnityEngine;

public class ExplosiveDamage : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private bool directInpact = false;
    [SerializeField] private float detonationTime = 2f;
    [SerializeField] private float explosionRadius = 2f; 

    private float damage; // Damage value passed from ProjectileThrowing

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    private void Update()
    {
        if (!directInpact)
        {
            detonationTime -= Time.deltaTime;

            if (detonationTime <= 0)
            {
                Explode();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!directInpact)
            return;

        Explode();
    }

    private void Explode()
    {
        // Detect all colliders within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D collider in colliders)
        {
            // Check if the collider has the NoteHealth component
            NodeHealth noteHealth = collider.GetComponent<NodeHealth>();
            BaseHealth baseHealth = collider.GetComponent<BaseHealth>();

            if (noteHealth != null)
            {
                // Apply damage to the note
                noteHealth.DamageNode(damage);
            }

            if (baseHealth != null)
            {
                CharacterStats characterStats = collider.GetComponent<CharacterStats>();
                baseHealth.TakeDamageWithInt(characterStats, (int)damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the explosion radius in the editor for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
