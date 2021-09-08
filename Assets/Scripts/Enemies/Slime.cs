using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BaseEnemyMoving))]
public class Slime : BaseEnemy
{
    private enum State
    {
        Idle,
        Move,
        Spin,
        Dead
    }

    [SerializeField] private float targetDetectionValue = 0.1f;
    [SerializeField] private Transform bottom;
    [SerializeField] private float idleTime;

    private BaseEnemyMoving slimeMoving;
    
    private State currentState;
    private Rigidbody2D rb;
    
    private float distance;
    private float idleTimer;
    
    private bool isSpinEnded;
    private bool isGrounded;

    protected override void Awake()
    {
        base.Awake();
        slimeMoving = GetComponent<BaseEnemyMoving>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if ((currentState != State.Dead)) CheckState();
        animator.SetBool(AnimationBoolNames.IsGrounded, isGrounded);

        animator.SetFloat(AnimationFloatNames.Velocity, Mathf.Abs(rb.velocity.x));
        
        UpdateCurrentState();
    }

    private void CheckState()
    {
        distance = Mathf.Abs(transform.position.x - slimeMoving.Target.x);
        isGrounded = Physics2D.OverlapCircle(bottom.position, 0.2f, LayerMask.GetMask(Layers.Ground));

        Debug.Log(idleTimer);
        if (!isGrounded|| (isGrounded && distance <= targetDetectionValue))
        {
            SetState(State.Idle);
        }
        else if (isGrounded && distance > targetDetectionValue && idleTimer <= 0)
        {
            SetState(State.Move);
            idleTimer = idleTime;

        }
        // else if (isGrounded && distance <= targetDetectionLenght)
        // {
        //     SetState(State.Idle);
        // }
    }

    private void UpdateCurrentState()
    {
        switch (currentState)
        {
            case State.Idle:
                UpdateIdle();

                break;
            case State.Move:
                UpdateMove();

                break;
            case State.Spin:
                UpdateSpin();

                break;
        }
    }

    private void UpdateSpin()
    {
        if (isSpinEnded)
        {
            SetState(State.Move);
        }
    }

    private void UpdateMove()
    {
    }

    private void UpdateIdle()
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

                //add is dead trigger
                break;
            case State.Spin:
                slimeMoving.enabled = false;
                IsInvulnerable = true;
                slimeMoving.IsTargetSet = false;
                slimeMoving.GetTarget();

                //add spin trigger
                break;
        }

        currentState = state;
    }
}
