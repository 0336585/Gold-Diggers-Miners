using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseMovement : MonoBehaviour
{
    [SerializeField] private float xSpeed = 5f;
    [SerializeField] private float ySpeed = 5f;
    [SerializeField] private float stairClimbForce = 5f; // Helps move up stairs smoothly
    [SerializeField] private LayerMask stairLayer; // Layer for stairs

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(float _x, float _y)
    {
        Vector2 velocity = new Vector2(_x * xSpeed, _y * ySpeed);

        // Apply stair climb force only if moving horizontally and on stairs
        if (_x != 0 && IsOnStairs())
        {
            velocity += Vector2.up * stairClimbForce * Time.fixedDeltaTime;
        }

        rb.linearVelocity = velocity;
    }

    public void SetVelocityX(float _x)
    {
        Vector2 velocity = new Vector2(_x * xSpeed, rb.linearVelocity.y);

        // Apply stair climb force only if moving horizontally and on stairs
        if (_x != 0 && IsOnStairs())
        {
            velocity += Vector2.up * stairClimbForce * Time.fixedDeltaTime;
        }

        rb.linearVelocity = velocity;
    }

    public void SetVelocityY(float _y)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, _y * ySpeed);
    }

    private bool IsOnStairs()
    {
        // Use a raycast or overlap check to detect if the player is on stairs
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, stairLayer);
        Debug.DrawRay(transform.position, Vector2.down * 1f, Color.red);
        return hit.collider != null;
    }
}