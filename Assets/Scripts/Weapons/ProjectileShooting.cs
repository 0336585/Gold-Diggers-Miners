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

    // TODO: Random delay when firing automatic

    private void Update()
    {
        // If automaticFire is true, shouldFire is true when "Fire1" is held down.
        // If automaticFire is false, shouldFire is true only when "Fire1" is first pressed.
        bool shouldFire = automaticFire ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");

        if (shouldFire && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
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

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        if (projectile.TryGetComponent(out Rigidbody2D rb))
        {
            rb.linearVelocity = firePoint.right * projectileSpeed;
        }
        Destroy(projectile, projectileLifetime);
    }
}
