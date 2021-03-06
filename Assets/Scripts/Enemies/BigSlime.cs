using System.Collections;
using UnityEngine;

public class BigSlime : Slime
{
    public bool isAnimationStarted;
    public bool isAnimationEnded;

    protected override void CheckDistance()
    {
        Distance = Vector3.Distance(transform.position, CurrentTargetPosition.position);

        if (Distance <= TargetDetectionValue)
        {
            if (!isAnimationStarted)
            {
                StartCoroutine(AnimationTime());

                IsInvulnerable = true;
                isAnimationStarted = true;
            }

            if (isAnimationEnded)
            {
                isAnimationStarted = false;
            }
        }
    }

    private IEnumerator AnimationTime()
    {
        Animator.SetTrigger(AnimationTriggerNames.Spin);

        yield return new WaitForSeconds(1);

        AIDestinationSetter.target = CurrentTargetPosition = CurrentTargetPosition == target1 ? target2 : target1;
        IsInvulnerable = false;
        isAnimationStarted = false;
    }
}
