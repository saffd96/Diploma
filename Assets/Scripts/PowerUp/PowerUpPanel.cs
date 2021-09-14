using System;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpPanel : MonoBehaviour
{
    [SerializeField] private PowerUp powerUp;

    [SerializeField] private Text powerUpName;
    [SerializeField] private Text description;
    [SerializeField] private Text buttonText;

    [SerializeField] private Image image;

    [SerializeField] private Button button;

    private PowerUp.Action actionWithPlayer;
    private PowerUpManager powerUpManager;

    private void Start()
    {
        powerUpManager = FindObjectOfType<PowerUpManager>();
        
        powerUpName.text = powerUp.PowerUpName;
        description.text = powerUp.Description;
        buttonText.text = "Select " + powerUp.PowerUpName;
        image.sprite = powerUp.Image;
        actionWithPlayer = powerUp.ActionWithPlayer;

        switch (actionWithPlayer)
        {
            case PowerUp.Action.AddLives:
                button.onClick.AddListener(powerUpManager.AddMaxLives);
                break;
            case PowerUp.Action.AddSpeed:
                button.onClick.AddListener(powerUpManager.AddSpeed);
                break;
        }
    }
}
