using UnityEngine;
using UnityEngine.UI;

public class StaminaBarView : MonoBehaviour
{
    private Slider staminaBar;
    private Text staminaText;

    private void Awake()
    {
        staminaBar = GetComponentInChildren<Slider>();
        staminaText = GetComponentInChildren<Text>();
    }

    public void SetValue(int value)
    {
        staminaBar.value = value;
        UpdateText(value);
    }

    public void SetMaxValue(int value)
    {
        staminaBar.maxValue = value;
        UpdateText(value);
    }

    private void UpdateText(int value)
    {
        staminaText.text = value.ToString();
    }
}
