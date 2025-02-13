using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;  
    [SerializeField] private Transform firePoint;
    [SerializeField] private bool automaticFiring = false;
    [SerializeField] private float projectileSpeed = 50f;
    [SerializeField] private float projectileLifetime = 0.3f;

    private void Update()
    {
        if (automaticFiring)
        {
            if (Input.GetButton("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.right * projectileSpeed;
            }
            Destroy(projectile, projectileLifetime);  
        }
        else
        {
            Debug.LogWarning("Projectile prefab or fire point is not assigned!");
        }
    }
}
