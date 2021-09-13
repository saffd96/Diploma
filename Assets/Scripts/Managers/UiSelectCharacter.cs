using System;
using UnityEngine;
using UnityEngine.UI;

public class UiSelectCharacter : MonoBehaviour
{
    [SerializeField] private Button startButton;
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
        startButton.gameObject.SetActive(toggle);

        foreach (var button in characterSelectionElements)
        {
            button.SetActive(!toggle);
        }
        toggle = !toggle;
    }
}
