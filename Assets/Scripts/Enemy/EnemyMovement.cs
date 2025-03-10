using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float returnDelay = 3f;

    [Header("Jump Settings")]
    [SerializeField] private bool canFly = false;
    [SerializeField] private float jumpForce = 1.2f;
    [SerializeField] private float jumpTolerance = 2f; // How much higher the destination must be to trigger a jump
    [SerializeField] private float obstacleCheckDistance = 2f; // How far ahead to check for obstacles

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck; // A transform positioned at the enemy's feet
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private EnemyTargeting enemyTargeting;
    private Vector2 originalPosition;
    private bool isReturning = false;
    private Rigidbody2D rb;
    private Coroutine returnCoroutine;
    private bool isJumpOnCooldown = false;

    private void Start()
    {
        enemyTargeting = GetComponent<EnemyTargeting>();
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (enemyTargeting.TargetPlayer != null)
        {
            if (returnCoroutine != null)
            {
                StopCoroutine(returnCoroutine);
                returnCoroutine = null;
                isReturning = false;
            }
            FollowPosition(enemyTargeting.TargetPlayer.transform.position);
        }
        else if (!isReturning)
        {
            returnCoroutine = StartCoroutine(ReturnToOriginalPosition());
        }
    }

    private void FollowPosition(Vector2 destination)
    {
        float direction = Mathf.Sign(destination.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if (!canFly && IsGrounded())
        {
            if (destination.y > transform.position.y + jumpTolerance)
            {
                Jump();
            }
            else
            {
                RaycastHit2D obstacleHit = Physics2D.Raycast(groundCheck.position, new Vector2(direction, 0), obstacleCheckDistance, groundLayer);
                if (obstacleHit.collider != null)
                {
                    Jump();
                }
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Jump()
    {
        if (IsGrounded() && !isJumpOnCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(JumpCooldownRoutine());
        }
    }

    private IEnumerator JumpCooldownRoutine()
    {
        isJumpOnCooldown = true;
        float cooldownDuration = Random.Range(0.5f, 1.25f);
        yield return new WaitForSeconds(cooldownDuration);
        isJumpOnCooldown = false;
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        isReturning = true;
        yield return new WaitForSeconds(returnDelay);

        while (Vector2.Distance(transform.position, originalPosition) > 0.1f)
        {
            FollowPosition(originalPosition);
            yield return null;
        }

        isReturning = false;
        returnCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        float direction = transform.localScale.x; // Assumes scale determines facing direction

        // Jump height check (yellow)
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + jumpTolerance));

        // Obstacle detection ray (blue)
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(direction * obstacleCheckDistance, 0, 0));

        // Ground check radius (cyan)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
