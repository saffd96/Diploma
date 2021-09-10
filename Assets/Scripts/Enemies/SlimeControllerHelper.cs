using UnityEngine;

public class SlimeControllerHelper : MonoBehaviour
{
    private BigSlime bigSlime;
    private Animator animator;

    private void Awake()
    {
        bigSlime = GetComponentInParent<BigSlime>();
        animator = GetComponent<Animator>();
    }

    private void EndSpin()
    {
        bigSlime.IsSpinEnded = true;
        animator.ResetTrigger(AnimationTriggerNames.Spin);
    }

}
