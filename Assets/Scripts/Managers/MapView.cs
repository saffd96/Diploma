using System;
using UnityEngine;
using DG.Tweening;

public class MapView : MonoBehaviour
{
    private Tweener tweenAnimation;

    private void Awake()
    {
        if (gameObject.activeSelf)
        {
           gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);

        //  tweenAnimation?.Kill();
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        //tweenAnimation?.Kill();
    }
}
