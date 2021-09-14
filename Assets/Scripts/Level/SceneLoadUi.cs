using UnityEngine;
using UnityEngine.UI;

public class SceneLoadUi : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text progressText;

    public void UpdateProgress(AsyncOperation operation)
    {
        var progress = Mathf.Clamp01(operation.progress / 0.9f);

        if (slider != null)
        {
            slider.value = progress;
        }

        if (progressText != null)
        {
            progressText.text = progress * 100f + "%";
        }
    }
}
