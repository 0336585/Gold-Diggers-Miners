using UnityEngine;

public class ExplosiveDamage : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private bool directImpact = false;
    [SerializeField] private float detonationTime = 2f;
    [SerializeField] private float explosionRadius = 2f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;  
    [SerializeField] private AudioClip fuseSound;  
    [SerializeField] private AudioClip explosionSound; 

    private float damage; // Damage value passed from ProjectileThrowing
    private bool fuseActive = false;
    private bool hasExploded = false; // Ensures explosion sound only plays once
    private float defaultVolume; // Stores the original AudioSource volume

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    private void Start()
    {
        if (audioSource != null)
        {
            defaultVolume = audioSource.volume; // Get the default volume from the AudioSource
        }

        if (!directImpact)
        {
            StartFuseSound();
        }
    }

    private void Update()
    {
        if (!directImpact)
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
        if (directImpact)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded) return; // Prevents multiple explosions
        hasExploded = true;

        StopFuseSound(); // Stop fuse sound before explosion

        // Play explosion sound at 1.5x the default volume
        PlayExplosionSound();

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

    private void StartFuseSound()
    {
        if (audioSource != null && fuseSound != null && !fuseActive)
        {
            fuseActive = true;
            InvokeRepeating(nameof(PlayFuseSound), 0f, 0.25f); // Play every 0.25 seconds
        }
    }

    private void StopFuseSound()
    {
        if (fuseActive)
        {
            CancelInvoke(nameof(PlayFuseSound));
            fuseActive = false;
        }
    }

    private void PlayFuseSound()
    {
        if (audioSource != null && fuseSound != null)
        {
            audioSource.PlayOneShot(fuseSound, defaultVolume); 
        }
    }

    private void PlayExplosionSound()
    {
        if (explosionSound != null)
        {
            // Play explosion sound at 6x the default volume at the object's position
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, defaultVolume * 6f);
        }
    }
}
