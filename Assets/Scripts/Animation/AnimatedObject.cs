using DG.Tweening;
using UnityEngine;

public abstract class AnimatedObject : MonoBehaviour
{
    protected Tween baseSequence;

    public abstract void PlayAnimation();
    public abstract void PlayReverseAnimation();
}
