using UnityEngine;
using UnityEngine.UI;

public class PowerUpPanel : MonoBehaviour
{
    [SerializeField] private Text powerUpName;
    [SerializeField] private Text description;
    [SerializeField] private Text buttonText;

    [SerializeField] private Image image;

    [SerializeField] private Button button;

    private PowerUp.Action actionWithPlayer;

    public void FillPanel(PowerUp powerUp)
    {
        var powerUpManager = FindObjectOfType<PowerUpManager>();
        var gameHandler = FindObjectOfType<GameHandler>();

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
            case PowerUp.Action.FillHp:
                button.onClick.AddListener(powerUpManager.FillHp);

                break;
            case PowerUp.Action.AddSpeed:
                button.onClick.AddListener(powerUpManager.AddSpeed);

                break;
            case PowerUp.Action.EnableRun:
                button.onClick.AddListener(powerUpManager.EnableRun);

                break;
            case PowerUp.Action.AddRunningSpeedMultiplier:
                button.onClick.AddListener(powerUpManager.AddRunningSpeedMultiplier);

                break;
            case PowerUp.Action.AddJumpForce:
                button.onClick.AddListener(powerUpManager.AddJumpForce);

                break;
            case PowerUp.Action.AddAdditionalJumps:
                button.onClick.AddListener(powerUpManager.AddAdditionalJump);

                break;
            case PowerUp.Action.EnableExtraJumps:
                button.onClick.AddListener(powerUpManager.EnableExtraJump);

                break;
            case PowerUp.Action.AddAttackValue:
                button.onClick.AddListener(powerUpManager.AddAttackValue);

                break;
            case PowerUp.Action.EnableRangeAttack:
                button.onClick.AddListener(powerUpManager.EnableRangeAttack);

                break;
            case PowerUp.Action.AddAdditionalStones:
                button.onClick.AddListener(powerUpManager.AddAdditionalStones);

                break;
        }

        button.onClick.AddListener(gameHandler.PowerUpPauseToggle);
    }
}
