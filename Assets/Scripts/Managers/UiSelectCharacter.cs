using UnityEngine;

public class UiSelectCharacter : MonoBehaviour
{
    [SerializeField] private AnimatedObject[] startThings;
    [SerializeField] private AnimatedObject[] characterSelectionElements;

    private bool toggle = true;

    private void Awake()
    {
        Toggle();
    }

    public void OnButtonPressed()
    {
        Toggle();
    }

    private void Toggle()
    {
        foreach (var thing in startThings)
        {
            if (toggle)
            {
                thing.PlayAnimation();
            }
            else
            {
                thing.PlayReverseAnimation();
            }
        }

        foreach (var button in characterSelectionElements)
        {
            if (!toggle)
            {
                button.PlayAnimation();
            }
            else
            {
                button.PlayReverseAnimation();
            }
        }

        toggle = !toggle;
    }
}
