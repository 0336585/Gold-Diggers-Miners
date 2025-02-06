using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region components
    [HideInInspector] public Rigidbody2D rb { get; private set; }
    [HideInInspector] public Animator animator { get; private set; }
    [HideInInspector] public BaseMovement baseMovement { get; private set; }
    [HideInInspector] protected BaseHealth healthScript;
    #endregion

    [field: SerializeField] public int facingDir { get; private set; } = 1;
    [field: SerializeField] public bool facingRight { get; private set; } = true;

    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    public System.Action onFlipped;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        baseMovement = GetComponent<BaseMovement>();
        healthScript = GetComponent<BaseHealth>();
    }

    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
            onFlipped();
    }

    public void ZeroVelocity() => rb.linearVelocity = Vector2.zero;

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
    #endregion
}
