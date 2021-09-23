using System;
using DG.Tweening;
using UnityEngine;

public class AnimatedPanel : AnimatedObject
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float baseAlpha = 1;
    [SerializeField] private float targetAlpha = 0;

    public event Action OnCompleteAnimation;
    public event Action OnCompleteReverseAnimation;

    private void Awake()
    {
        canvasGroup.alpha = 0;
    }

    public override void PlayAnimation()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        baseSequence?.Kill();
        canvasGroup.DOFade(targetAlpha, fadeDuration).SetUpdate(true).OnComplete(CompleteAnimation);
    }

    public override void PlayReverseAnimation()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        baseSequence?.Kill();

        var sequence = DOTween.Sequence().SetUpdate(UpdateType.Fixed);
        canvasGroup.DOFade(baseAlpha, fadeDuration).SetUpdate(true).OnComplete(CompleteReverseAnimation);

        baseSequence = sequence;
    }

    private void CompleteAnimation()
    {
        OnCompleteAnimation?.Invoke();
    }

    private void CompleteReverseAnimation()
    {
        OnCompleteReverseAnimation?.Invoke();
    }
}
