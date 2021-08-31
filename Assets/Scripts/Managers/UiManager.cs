using System;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private PauseView pauseView;
    [SerializeField] private MapView mapView;

    [SerializeField] private Image mapUIImage;
    [SerializeField] private Sprite[] mapUISprites = new Sprite[2];

    private void Start()
    {
        pauseView.enabled = false;
    }

    public void PauseToggle(bool isActive)
    {
        if (isActive)
        {
            pauseView.Show();
        }
        else
        {
            pauseView.Hide();
        }
    }
    
    public void MapToggle(bool isActive)
    {
        if (isActive)
        {
            mapUIImage.sprite = mapUISprites[0];
            mapView.Show();
        }
        else
        {           
            mapUIImage.sprite = mapUISprites[1];
            mapView.Hide();
        }
    }
}
