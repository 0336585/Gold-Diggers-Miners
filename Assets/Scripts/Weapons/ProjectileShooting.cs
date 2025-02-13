using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private bool automaticFire;
    [SerializeField] private float projectileSpeed = 25f;
    [SerializeField] private float projectileLifetime = 0.3f;
    [SerializeField] private float fireRate = 0.4f;  
    private float nextFireTime = 0f;

    private void Update()
    {
        if (automaticFire)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
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