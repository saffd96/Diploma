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

    [SerializeField] private float targetDetectionLenght;
    [SerializeField] private Transform bottom;

    private BaseEnemyMoving slimeMoving;
    private State currentState;
    private float distance;
    private bool isGrounded;

    private bool isSpinEnded;

    protected override void Awake()
    {
        base.Awake();
        slimeMoving = GetComponent<BaseEnemyMoving>();
    }

    private void Start()
    {
        SetState(State.Idle);
    }

    private void Update()
    {
        if ((currentState != State.Dead)) CheckState();
        animator.SetBool(AnimationBoolNames.IsGrounded, isGrounded);
        Debug.Log(currentState);

        UpdateCurrentState();

        // Debug.Log(distance);
        //Debug.Log($"{currentState}{distance}");
    }

    private void CheckState()
    {
        distance = Mathf.Abs(transform.position.x - slimeMoving.Target.x);
        isGrounded = Physics2D.OverlapCircle(bottom.position, 0.2f, LayerMask.GetMask(Layers.Ground));

        if (!isGrounded)
        {
            SetState(State.Idle);
        }
        else if (isGrounded && distance > targetDetectionLenght)
        {
            SetState(State.Move);
        }
        else if (isGrounded && distance <= targetDetectionLenght)
        {
            SetState(State.Spin);
        }
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
    }

    private void SetState(State state)
    {
        switch (state)
        {
            case State.Idle:
                slimeMoving.enabled = false;

                break;
            case State.Move:
                slimeMoving.enabled = true;
                slimeMoving.GetTarget();

                break;
            case State.Dead:
                slimeMoving.enabled = false;

                //add is dead trigger
                break;
            case State.Spin:
                slimeMoving.enabled = false;
                slimeMoving.GetTarget();

                //add spin trigger
                break;
        }

        currentState = state;
    }
}
