using System.Collections;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [Header("Basic Variables")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Fire Modes")]
    [SerializeField] private bool automaticFire = false;
    [SerializeField] private bool multishotMode = false;

    [Header("General Settings")]
    //TODO: Damage doesn't do anything yet
    [SerializeField] private float damagePerShot = 20f;
    [SerializeField] private float projectileSpeed = 25f;
    [SerializeField] private float projectileLifetime = 0.3f;
    [SerializeField] private float fireRate = 0.4f;
    [SerializeField] private float randomFireVariation = 0.1f;

    [Header("Ammo Settings")]
    [SerializeField] private int ammoInGun = 6;
    [SerializeField] private int reserveAmmo = 30;
    //TODO: Add private max ammo that's a check for reserveAmmo while being the same amount 
    [SerializeField] private float reloadTime = 2f;

    [Header("Multishot Settings")]
    [SerializeField] private int bulletsPerShot = 5;
    [SerializeField, Range(0f, 360f)] private float spreadAngle = 20f;

    private int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    private void Start()
    {
        currentAmmo = ammoInGun;  
    }

    private void Update()
    {
        if (isReloading) return; // Cannot fire while reloading

        // If automaticFire is true, shouldFire is true when "Fire1" is held down.
        // If automaticFire is false, shouldFire is true only when "Fire1" is first pressed.
        bool shouldFire = automaticFire ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");

        if (shouldFire && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                //An random range between - 0.1 and 0.1, too simulate irregularities in these old guns
                float randomDelay = Random.Range(-randomFireVariation, randomFireVariation);
                nextFireTime = Time.time + fireRate + randomDelay;
                Shoot();
                currentAmmo--;  
            }
            else
            {
                Debug.Log("Out of ammo! Press 'R' to reload.");
            }
        }

        // Reload when "R" is pressed
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < ammoInGun && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("Projectile prefab or fire point is not assigned!");
            return;
        }

        if (multishotMode)
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

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);  

        int ammoNeeded = ammoInGun - currentAmmo; // How many bullets we need to refill
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo); // Take from total ammo

        currentAmmo += ammoToReload; // Fill the gun with bullets
        reserveAmmo -= ammoToReload; // Reduce total ammo

        isReloading = false;
        Debug.Log("Reload complete!");
        Debug.Log("currentAmmo: " + currentAmmo);
        Debug.Log("reserveAmmo: " + reserveAmmo);
    }
}
