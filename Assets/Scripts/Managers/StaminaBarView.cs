using UnityEngine;
using UnityEngine.UI;

public class StaminaBarView : MonoBehaviour
{
    private Slider staminaBar;

    private void Awake()
    {
        staminaBar = GetComponentInChildren<Slider>();
    }

    public void SetValue(int value)
    {
        staminaBar.value = value;
    }

    public void SetMaxValue(int value)
    {
        staminaBar.maxValue = value;
    }
}
