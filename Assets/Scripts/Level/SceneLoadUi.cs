using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SceneLoadUi : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private Text progressText;
    [SerializeField] private Gradient gradient;
    [SerializeField] private RectTransform vfxTransform;
    
    private float currentProgress;

    private void OnDrawGizmos()
    {
        barImage.color = gradient.Evaluate(barImage.fillAmount);
        progressText.text = $"{barImage.fillAmount:P1}";
    }

    public void UpdateProgress()
    {
        if (barImage != null)
        {
            barImage.color = gradient.Evaluate(barImage.fillAmount);

            if (vfxTransform!=null)
            {
                vfxTransform.anchoredPosition = new Vector2(barImage.fillAmount / -0.5f *barImage.rectTransform.rect.x, 0);
            }
            
            if (barImage.fillAmount >= 0.9f)
            {
                return;
            }
        
            barImage.fillAmount = currentProgress += Random.Range(0f, 0.0225f);
        }
        
        if (progressText != null)
        {
            progressText.text = $"{barImage.fillAmount:P1}";
        }
    }

    public void CompleteProgress()
    {
        if (barImage != null)
        {
            barImage.fillAmount = 1f;
            
            if (vfxTransform!=null)
            {
                vfxTransform.gameObject.SetActive(false);
            }
        }
        
        if (progressText != null)
        {
            progressText.text = $"{barImage.fillAmount:P1}";
        }
    }
}
