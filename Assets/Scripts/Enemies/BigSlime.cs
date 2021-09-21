using System.Collections;
using UnityEngine;

public class BigSlime : Slime
{
    public bool isAnimationStarted;
    public bool isAnimationEnded;

    protected override void CheckDistance()
    {
        Debug.Log(isAnimationStarted);
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
    
    // protected override void ChangeTarget()
    // {
    //     if (IsSpinEnded)
    //     {
    //         AIDestinationSetter.target = CurrentTargetPosition = CurrentTargetPosition == target1 ? target2 : target1;
    //         IsInvulnerable = false;
    //         IsSpinEnded = false;
    //     }
    // }

    private IEnumerator AnimationTime()
    {
        Animator.SetTrigger(AnimationTriggerNames.Spin);
        yield return new WaitForSeconds(1);
        
        AIDestinationSetter.target = CurrentTargetPosition = CurrentTargetPosition == target1 ? target2 : target1;
        IsInvulnerable = false;
        isAnimationStarted = false;
    }

}
