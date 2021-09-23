using UnityEngine;
using DG.Tweening;

public class MapView : MonoBehaviour
{
    [SerializeField] private AnimatedObject animatedObject;

    private Tweener tweenAnimation;

    public void Show()
    {
        gameObject.SetActive(true);
        AudioManager.Instance.PlayButtonOnClickSfx();
        tweenAnimation?.Kill();
        animatedObject.PlayAnimation();
    }

    public void Hide()
    {
        AudioManager.Instance.PlayButtonOnClickSfx();
        tweenAnimation?.Kill();
        animatedObject.PlayReverseAnimation();
    }
}
