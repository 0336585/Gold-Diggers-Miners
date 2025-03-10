using System.Collections;
using UnityEngine;

public class ProjectileThrowing : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwDelay = 0.5f;

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 5;
    private int currentAmmo;

    private bool canThrow = true;
    private bool firstThrow = true;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && canThrow && currentAmmo > 0)
        {
            StartCoroutine(ThrowWithDelay());
        }
    }

    private IEnumerator ThrowWithDelay()
    {
        canThrow = false;

        // No delay on the first throw
        if (!firstThrow)
        {
            yield return new WaitForSeconds(throwDelay);
        }
        else
        {
            firstThrow = false; // After first throw, future throws use delay
        }

        ThrowProjectile();
        canThrow = true;
    }

    private void ThrowProjectile()
    {
        if (currentAmmo <= 0) return; // Don't throw if no ammo

        GameObject projectile = Instantiate(projectilePrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Get the aim direction from mouse position (or right stick on a controller)
            Vector3 aimDirection = (GetAimDirection() - throwPoint.position).normalized;

            // Apply velocity based on aim direction and throw force
            rb.linearVelocity = aimDirection * throwForce;

            // Rotate projectile to match throw direction
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Pass the damage value to the ExplosiveDamage script
            ExplosiveDamage explosiveDamage = projectile.GetComponent<ExplosiveDamage>();
            if (explosiveDamage != null)
            {
                explosiveDamage.SetDamage(damage);
            }

            // Reduce ammo after throwing
            currentAmmo--;
            //Debug.Log(gameObject.name + " Ammo left: " + currentAmmo);
        }
        else
        {
            Debug.Log("Add a Rigidbody to the prefab, dipshit >:(");
        }
    }

    private Vector3 GetAimDirection()
    {
        // Convert mouse position to world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Keep it in 2D

        return mousePos;
    }
}
