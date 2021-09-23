using DG.Tweening;
using UnityEngine;

public class AnimatedElement : AnimatedObject
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private float moveTime = 0.5f;
    [SerializeField] private Ease toStartPointEase = Ease.Linear;
    [SerializeField] private Ease toEndPointEase = Ease.Linear;
    [SerializeField] private int loops = 1;

    public override void PlayAnimation()
    {
        baseSequence?.Kill();

        baseSequence = transform.DOLocalMove(endPosition, moveTime).SetUpdate(true).SetEase(toEndPointEase)
               .SetLoops(loops);
    }

    public override void PlayReverseAnimation()
    {
        baseSequence?.Kill();

        var sequence = DOTween.Sequence().SetUpdate(UpdateType.Fixed);

        baseSequence = transform.DOLocalMove(startPosition, moveTime).SetUpdate(true).SetEase(toStartPointEase)
               .SetLoops(loops);

        baseSequence = sequence;
    }
}
