using System.Collections;
using UnityEngine;

public class ProjectileThrowing : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwAngle = 45f;
    [SerializeField] private float throwDelay = 0.5f;  

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 5; // Maximum amount of ammo
    private int currentAmmo; // Tracks the current ammo count

    [Header("Player Settings")]
    [SerializeField] private Transform player; // Reference to the player to determine direction

    private bool canThrow = true;
    private bool firstThrow = true; // Tracks if it's the first throw

    private void Start()
    {
        // Initialize ammo count to max at start
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
            // Determine the direction the player is facing (1 for right, -1 for left)
            float direction = player.localScale.x > 0 ? 1f : -1f;
            // Convert the throw angle from degrees to radians for trigonometric calculations (basically SosCasToa)
            float angleInRadians = throwAngle * Mathf.Deg2Rad;

            // Calculate the initial velocity components based on the throw angle and force
            // xVelocity determines the horizontal movement, adjusting for player direction
            // yVelocity determines the vertical movement, creating the arc effect
            float xVelocity = Mathf.Cos(angleInRadians) * throwForce * direction;
            float yVelocity = Mathf.Sin(angleInRadians) * throwForce;

            // Apply velocity
            rb.linearVelocity = new Vector2(xVelocity, yVelocity);

            // Reduce ammo after throwing
            currentAmmo--;
            Debug.Log(gameObject + " Ammo left: " + currentAmmo);
        }
        else
        {
            Debug.Log("Add a Rigidbody to the prefab, dipshit >:(");
        }
    }
}
