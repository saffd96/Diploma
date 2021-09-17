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

    protected float distance;
    private float idleTimer;
    
    protected bool isGrounded;
    protected BaseEnemyMoving SlimeMoving => slimeMoving;

    protected override void Awake()
    {
        base.Awake();
        slimeMoving = GetComponent<BaseEnemyMoving>();
    }

    private void Update()
    {
        if (currentState == State.Dead) return;

        CheckState();

        Animator.SetBool(AnimationBoolNames.IsGrounded, isGrounded);

        Animator.SetFloat(AnimationFloatNames.Velocity, Mathf.Abs(rb2D.velocity.x));

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

                rb2D.velocity = Vector2.zero;
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

                Animator.SetBool(AnimationBoolNames.IsDead, true);
                coll2D.enabled = false;
                rb2D.Sleep();              
                break;
        }

        currentState = state;
    }
}
