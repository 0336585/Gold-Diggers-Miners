using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    private float damageAmount;

    private void OnCollisionEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the NoteHealth component
        NoteHealth noteHealth = collision.GetComponent<NoteHealth>();

        if (noteHealth != null)
        {
            noteHealth.DamageNote(damageAmount);
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