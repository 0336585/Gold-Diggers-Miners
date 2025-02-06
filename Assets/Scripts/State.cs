using UnityEngine;

public class State
{
    protected string animBoolName;
    protected Animator animator;
    protected Rigidbody2D rb;

    protected float stateTimer;

    public virtual void Enter()
    {
        animator.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Exit()
    {
        animator.SetBool(animBoolName, false);
    }
}
