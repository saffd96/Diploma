using UnityEngine;
using DG.Tweening;

public class MapView : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.2f;

    private Tweener tweenAnimation;

    private void Awake()
    {
        canvasGroup.alpha = 0;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        AudioManager.Instance.PlayButtonOnClickSfx();
        tweenAnimation?.Kill();
        canvasGroup.DOFade(1, fadeDuration).SetUpdate(true);
    }

    public void Hide()
    {
        AudioManager.Instance.PlayButtonOnClickSfx();
        tweenAnimation?.Kill();
        canvasGroup.DOFade(0, fadeDuration).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
    }
}
