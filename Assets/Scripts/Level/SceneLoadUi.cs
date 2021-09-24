using UnityEngine;
using UnityEngine.UI;

public class SceneLoadUi : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text progressText;

    private float currentProgress;

    public void UpdateProgress()
    {
        if (slider != null)
        {
            if (slider.value >= 0.9f)
            {
                return;
            }

            slider.value = currentProgress += Random.Range(0f, 0.025f);
        }

        if (progressText != null)
        {
            progressText.text = $"{slider.value:P1}";
        }
    }

    public void SetProgress()
    {
        if (slider != null)
        {
            slider.value = 1f;
        }

        if (progressText != null)
        {
            progressText.text = $"{slider.value:P1}";
        }
    }
}
