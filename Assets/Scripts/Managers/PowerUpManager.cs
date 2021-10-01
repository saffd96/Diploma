using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using PlayerComponents;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private PowerUpPanel panelPrefab;
    [SerializeField] private Transform[] panelPositions;
    [SerializeField] private List<PowerUp> powerUps;

    [Space]
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private float fadeDuration = 0.2f;

    private Tweener tweenAnimation;
    private PlayerPowerUp playerPowerUp;

    private List<PowerUpPanel> powerUpPanels = new List<PowerUpPanel>();
    private List<PowerUp> usedPowerUps = new List<PowerUp>();
    private PowerUp tempPowerUp;

    private void Start()
    {
        playerPowerUp = FindObjectOfType<PlayerPowerUp>();
        usedPowerUps.Clear();

        for (int i = 0; i < panelPositions.Length; i++)
        {
            var panel = Instantiate(panelPrefab, panelPositions[i]);

            powerUpPanels.Add(panel);

            panel.FillPanel(SelectPowerUp());
            panel.PlayAnimation();
        }
    }

    private PowerUp SelectPowerUp()
    {
        tempPowerUp = powerUps[Random.Range(0, powerUps.Count)];

        if (usedPowerUps.Count == 0)
        {
            usedPowerUps.Add(tempPowerUp);

            return tempPowerUp;
        }

        while (usedPowerUps.Contains(tempPowerUp))
        {
            tempPowerUp = powerUps[Random.Range(0, powerUps.Count)];
        }

        usedPowerUps.Add(tempPowerUp);

        return tempPowerUp;
    }

    public void AddMaxLives()
    {
        playerPowerUp.HealthPowerUp();
    }

    public void FillHp()
    {
        playerPowerUp.HealthPowerUp();
    }

    public void AddSpeed()
    {
        playerPowerUp.AddSpeed();
    }

    public void EnableRun()
    {
        playerPowerUp.RunPowerUp();
    }

    public void AddRunningSpeedMultiplier()
    {
        playerPowerUp.RunPowerUp();
    }

    public void AddJumpForce()
    {
        playerPowerUp.AddJumpForce();
    }

    public void AddAdditionalJump()
    {
        playerPowerUp.AdditionalJumpsPowerUp();
    }

    public void EnableExtraJump()
    {
        playerPowerUp.AdditionalJumpsPowerUp();
    }

    public void AddAttackValue()
    {
        playerPowerUp.AddAttackValue();
    }

    public void AddAdditionalStones()
    {
        playerPowerUp.RangedPowerUp();
    }

    public void EnableRangeAttack()
    {
        playerPowerUp.RangedPowerUp();
    }

    public void Hide()
    {
        tweenAnimation?.Kill();
        canvasGroup.DOFade(0, fadeDuration).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
    }
}
