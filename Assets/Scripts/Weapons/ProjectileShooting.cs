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
    [SerializeField] private float damagePerShot = 20f;
    [SerializeField] private float projectileSpeed = 25f;
    [SerializeField] private float projectileLifetime = 0.3f;
    [SerializeField] private float fireRate = 0.4f;
    [SerializeField] private float randomFireVariation = 0.1f;

    [Header("Ammo Settings")]
    [SerializeField] private int ammoInGun = 6;
    [SerializeField] private int reserveAmmo = 30;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip emptyAmmoSound;

    public int ReserveAmmo
    {
        get { return reserveAmmo; }
        private set { reserveAmmo = value; }
    }

    [SerializeField] private float reloadTime = 2f;

    [Header("Multishot Settings")]
    [SerializeField] private int bulletsPerShot = 5;
    [SerializeField, Range(0f, 360f)] private float spreadAngle = 20f;

    private int currentAmmo;
    private int maxAmmo;
    private bool isReloading = false;

    public int CurrentAmmo
    {
        get { return currentAmmo; }
        private set { currentAmmo = value; }
    }

    public int MaxAmmo
    {
        get { return maxAmmo; }
    }

    public bool IsReloading
    {
        get { return isReloading; }
        private set { isReloading = value; }
    }

    private float nextFireTime = 0f;

    private void Start()
    {
        currentAmmo = ammoInGun;
        maxAmmo = reserveAmmo;
    }

    private void Update()
    {
        if (MenuManager.Instance.inMenu) return;

        if (isReloading) return; // Cannot fire while reloading

        // If automaticFire is true, shouldFire is true when "Fire1" is held down.
        // If automaticFire is false, shouldFire is true only when "Fire1" is first pressed.
        bool shouldFire = automaticFire ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");

        if (shouldFire && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                // A random range between -0.1 and 0.1, to simulate irregularities in these old guns
                float randomDelay = Random.Range(-randomFireVariation, randomFireVariation);
                nextFireTime = Time.time + fireRate + randomDelay;
                Shoot();
                currentAmmo--;
                PlaySound(fireSound);
            }
            else
            {
                // Check if enough time has passed since the last empty fire sound
                if (Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + 0.2f; // Set the next allowed time to play the empty fire sound
                    PlaySound(emptyAmmoSound);
                }
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

        bool isFlipped = firePoint.lossyScale.x < 0; // Check if arm (fire point) is flipped

        // Calculate direction to mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure 2D space
        Vector2 shootDirection = (mousePosition - firePoint.position).normalized;

        if (multishotMode)
        {
            // Shotgun mode: Fire multiple bullets in a spread
            for (int i = 0; i < bulletsPerShot; i++)
            {
                // Generate a random spread angle for each bullet
                float randomAngle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
                if (isFlipped)
                {
                    randomAngle = -randomAngle; // Flip the spread angle when facing left
                }

                // Apply the spread by rotating the original shoot direction
                Quaternion spreadRotation = Quaternion.Euler(0, 0, randomAngle);

                // Adjust the shoot direction with the spread rotation
                Vector2 spreadDirection = spreadRotation * shootDirection;

                // Create and shoot the projectile
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(Vector3.forward, spreadDirection));
                SetProjectileDamage(projectile);
                projectile.GetComponent<ProjectileDamage>().SetShooter(this);

                if (projectile.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.linearVelocity = spreadDirection * projectileSpeed;
                }

                Destroy(projectile, projectileLifetime);
            }
        }
        else
        {
            // Rifle mode: Fire a single bullet  
            Quaternion bulletRotation = Quaternion.Euler(0, 0, Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg);
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, bulletRotation);
            SetProjectileDamage(projectile);
            projectile.GetComponent<ProjectileDamage>().SetShooter(this);

            if (projectile.TryGetComponent(out Rigidbody2D rb))
            {
                rb.linearVelocity = shootDirection * projectileSpeed;
            }

            Destroy(projectile, projectileLifetime);
        }
    }

    private void SetProjectileDamage(GameObject projectile)
    {
        if (projectile.TryGetComponent(out ProjectileDamage projectileDamage))
        {
            projectileDamage.SetDamage(damagePerShot);
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        StartCoroutine(PlayReloadSoundLoop()); // Start looping the reload sound

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = ammoInGun - currentAmmo; // How many bullets we need to refill
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo); // Take from total ammo

        currentAmmo += ammoToReload; // Fill the gun with bullets
        reserveAmmo -= ammoToReload; // Reduce total ammo

        isReloading = false;
    }

    // Coroutine to continuously play the reload sound
    private IEnumerator PlayReloadSoundLoop()
    {
        while (isReloading)
        {
            PlaySound(reloadSound);
            yield return new WaitForSeconds(0.35f); // Interval between each sound
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
