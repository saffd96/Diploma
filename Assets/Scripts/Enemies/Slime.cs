using UnityEngine;

[RequireComponent(typeof(BaseEnemyMoving))]
public class Slime : BaseEnemy
{
    private enum State
    {
        Idle,
        Move,
        Dead
    }

    [SerializeField] protected float targetDetectionValue = 0.1f;
    [SerializeField] protected Transform bottom;
    [SerializeField] protected float idleTime;

    protected BaseEnemyMoving slimeMoving;

    private State currentState;
    protected Rigidbody2D rb;

    protected float distance;
    private float idleTimer;
    private Collider2D coll2D;
    
    protected bool isGrounded;
    protected BaseEnemyMoving SlimeMoving => slimeMoving;

    protected override void Awake()
    {
        base.Awake();
        slimeMoving = GetComponent<BaseEnemyMoving>();
        coll2D = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Debug.Log(currentState);
        
        if (currentState == State.Dead) return;

        CheckState();

        animator.SetBool(AnimationBoolNames.IsGrounded, isGrounded);

        animator.SetFloat(AnimationFloatNames.Velocity, Mathf.Abs(rb.velocity.x));

        UpdateCurrentState();
    }

    protected override void Die()
    {
        base.Die();
        SetState(State.Dead);
    }

    protected virtual void CheckState()
    {
        distance = Mathf.Abs(transform.position.x - slimeMoving.Target.x);
        isGrounded = Physics2D.OverlapCircle(bottom.position, targetDetectionValue, LayerMask.GetMask(Layers.Ground));

        
        if (!isGrounded || distance <= targetDetectionValue)
        {
            SetState(State.Idle);
        }
        else if (isGrounded && distance > targetDetectionValue && idleTimer <= 0)
        {
            SetState(State.Move);
            idleTimer = idleTime;
        }
    }

    protected virtual void UpdateCurrentState()
    {
        switch (currentState)
        {
            case State.Idle:
                UpdateIdle();

                break;
            case State.Move:
                UpdateMove();

                break;
        }
    }

    protected virtual void UpdateMove()
    {
    }

    protected virtual void UpdateIdle()
    {
        idleTimer -= Time.deltaTime;

        if (idleTimer <= 0)
        {
            slimeMoving.GetTarget();
        }
    }

    private void SetState(State state)
    {
        switch (state)
        {
            case State.Idle:

                rb.velocity = Vector2.zero;
                slimeMoving.enabled = false;
                slimeMoving.IsTargetSet = false;

                break;
            case State.Move:
                slimeMoving.enabled = isGrounded;
                IsInvulnerable = false;
                slimeMoving.GetTarget();

                break;
            case State.Dead:
                slimeMoving.enabled = false;

                animator.SetBool(AnimationBoolNames.IsDead, true);
                coll2D.enabled = false;
                rb.Sleep();              
                break;
        }

        currentState = state;
    }
}
