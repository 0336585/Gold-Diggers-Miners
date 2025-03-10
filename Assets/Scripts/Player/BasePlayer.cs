using UnityEngine;

[RequireComponent(typeof(BaseMovement))]
public class BasePlayer : Entity
{
    private StateMachine stateMachine;

    public static BasePlayer Instance;

    [Header("Movement info")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;
    [HideInInspector] private float dashTimer;
    [SerializeField] private float climbSpeed;
    [HideInInspector] private bool inClimbRange;

    #region States
    [HideInInspector] public PlayerState idleState { get; private set; }
    [HideInInspector] public PlayerState walkState { get; private set; }
    [HideInInspector] public PlayerState sprintState { get; private set; }
    [HideInInspector] public PlayerState dashState { get; private set; }
    [HideInInspector] public PlayerState jumpState { get; private set; }
    [HideInInspector] public PlayerState fallingState { get; private set; }
    #endregion


    public override void Awake()
    {
        base.Awake();

        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate inventory instances
            return;
        }

        Instance = this;

        healthScript = GetComponent<PlayerHealth>();

        stateMachine = new StateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        walkState = new PlayerWalkState(this, stateMachine, "Run");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump", jumpForce);
        fallingState = new PlayerFallingState(this, stateMachine, "Falling");
        dashState = new PlayerDashState(this, stateMachine, "Dash", dashSpeed);

        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();

        //Debug.Log(stateMachine.currentState.GetType().Name);

        DashCheck();
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.FixedUpdate();
    }

    #region Checks

    private void DashCheck()
    {
        dashTimer -= Time.deltaTime;

        if (dashTimer > 0) return;

        if (Input.GetKeyDown(KeyCode.Q) && stateMachine.currentState != dashState)
        {
            dashTimer = dashCooldown;
            stateMachine.ChangeState(dashState);
        }
    }
    #endregion
}
