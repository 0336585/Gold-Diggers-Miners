using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [Header("Basic Variables")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Fire Modes")]
    [SerializeField] private bool automaticFire;
    [SerializeField] private bool multishotFire;

    [Header("General Settings")]
    [SerializeField] private float projectileSpeed = 25f;
    [SerializeField] private float projectileLifetime = 0.3f;
    [SerializeField] private float fireRate = 0.4f;
    [SerializeField] private float randomFireVariation = 0.1f;

    [Header("Multishot Settings")]
    [SerializeField] private int bulletsPerShot = 5;  
    [SerializeField, Range(0f, 360f)] private float spreadAngle = 25f;  

    private float nextFireTime = 0f;

    private void Update()
    {
        // If automaticFire is true, shouldFire is true when "Fire1" is held down.
        // If automaticFire is false, shouldFire is true only when "Fire1" is first pressed.
        bool shouldFire = automaticFire ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");

        if (shouldFire && Time.time >= nextFireTime)
        {
            // A random range between -0.1 and 0.1, to simulate irregularities in these old guns
            float randomDelay = Random.Range(-randomFireVariation, randomFireVariation);
            nextFireTime = Time.time + fireRate + randomDelay;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("Projectile prefab or fire point is not assigned!");
            return;
        }

        if (multishotFire)
        {
            // Shotgun mode: Fire multiple bullets in a spread
            for (int i = 0; i < bulletsPerShot; i++)
            {
                float randomAngle = Random.Range(-spreadAngle / 2, spreadAngle / 2); // Random angle within the spread
                Quaternion spreadRotation = Quaternion.Euler(0, 0, randomAngle); // Create a rotation offset
                Quaternion bulletRotation = firePoint.rotation * spreadRotation; // Apply offset to the firePoint rotation

                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, bulletRotation);
                if (projectile.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.linearVelocity = bulletRotation * Vector2.right * projectileSpeed; // Apply spread rotation to velocity
                }
                Destroy(projectile, projectileLifetime);
            }
        }
        else
        {
            // Rifle mode: Fire a single bullet
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            if (projectile.TryGetComponent(out Rigidbody2D rb))
            {
                rb.linearVelocity = firePoint.right * projectileSpeed;
            }
            Destroy(projectile, projectileLifetime);
        }
    }
}
