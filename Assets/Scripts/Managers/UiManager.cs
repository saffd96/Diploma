using System;
using PlayerComponents;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private PauseView pauseView;
    [SerializeField] private PowerUpManager powerUpView;
    [SerializeField] private MapView mapView;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private StoneView stoneView;
    [SerializeField] private StaminaBarView staminaBarView;

    [Space]
    [SerializeField] private GameObject rangeAttackImage;
    [SerializeField] private GameObject aimTargetImage;
    [SerializeField] private Image mapUIImage;
    [SerializeField] private Sprite[] mapUISprites = new Sprite[2];

    private void OnEnable()
    {
        PlayerPowerUp.OnRunPowerUp += ShowStamina;
    }

    private void OnDisable()
    {
        PlayerPowerUp.OnRunPowerUp -= ShowStamina;
    }

    private void Start()
    {
        staminaBarView.gameObject.SetActive(false);
    }

    private void Update()
    {
        rangeAttackImage.SetActive(stoneView.CanvasGroup.alpha >= 1);
        aimTargetImage.SetActive(stoneView.CanvasGroup.alpha >= 1);
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

    public void PowerUpHide()
    {
        powerUpView.Hide();
    }

    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
    }

    private void ShowStamina()
    {
        staminaBarView.gameObject.SetActive(true);
    }
}
