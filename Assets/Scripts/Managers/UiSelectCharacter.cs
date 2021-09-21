using System;
using UnityEngine;
using UnityEngine.UI;

public class UiSelectCharacter : MonoBehaviour
{
    [SerializeField] private Button[] startButtons;
    [SerializeField] private GameObject[] characterSelectionElements;

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
        foreach (var button in startButtons)
        {
            button.gameObject.SetActive(toggle);
        }

        foreach (var button in characterSelectionElements)
        {
            button.SetActive(!toggle);
        }
        toggle = !toggle;
    }
}
