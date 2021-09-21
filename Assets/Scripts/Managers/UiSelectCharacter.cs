using System;
using UnityEngine;
using UnityEngine.UI;

public class UiSelectCharacter : MonoBehaviour
{
    [SerializeField] private GameObject[] startThings;
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
        foreach (var thing in startThings)
        {
            thing.SetActive(toggle);
        }

        foreach (var button in characterSelectionElements)
        {
            button.SetActive(!toggle);
        }

        toggle = !toggle;
    }
}
