using UnityEngine;

public class BigSlime : Slime
{
    private enum State
    {
        Idle,
        Move,
        Spin,
        Dead
    }

    private State currentBigSlimeState;
    public bool IsSpinEnded { get; set; }

    protected override void CheckState()
    {
        distance = Mathf.Abs(transform.position.x - slimeMoving.Target.x);
        isGrounded = Physics2D.OverlapCircle(bottom.position, 0.2f, LayerMask.GetMask(Layers.Ground));

        Debug.Log(currentBigSlimeState);
        Debug.Log(rb.velocity);
        Debug.Log(IsInvulnerable);

        if (!isGrounded)
        {
            SetState(State.Idle);
        }
        else if (isGrounded && distance > targetDetectionValue)
        {
            SetState(State.Move);
        }
        else if (isGrounded && distance <= targetDetectionValue)
        {
            SetState(State.Spin);
        }
    }

    protected override void UpdateCurrentState()
    {
        switch (currentBigSlimeState)
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
    }

    protected override void UpdateMove()
    {
    }

    protected override void UpdateIdle()
    {
    }

    private void SetState(State state)
    {
        switch (state)
        {
            case State.Idle:
                slimeMoving.enabled = isGrounded;
                slimeMoving.IsTargetSet = false;
                IsSpinEnded = false;

                break;
            case State.Move:
                IsSpinEnded = false;
                slimeMoving.enabled = true;
                IsInvulnerable = false;
                slimeMoving.GetTarget();

                break;
            case State.Dead:
                slimeMoving.enabled = false;

                //add is dead trigger
                break;
            case State.Spin:
                if (!IsSpinEnded)
                {
                    animator.SetTrigger(AnimationTriggerNames.Spin);
                }

                slimeMoving.IsTargetSet = false;
                slimeMoving.enabled = false;
                IsInvulnerable = true;
                rb.velocity = Vector2.zero;

                if (IsSpinEnded)
                {
                    SlimeMoving.GetTarget();
                }

                break;
        }

        currentBigSlimeState = state;
    }
}
