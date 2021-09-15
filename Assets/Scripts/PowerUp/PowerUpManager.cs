using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private PowerUpPanel panelPrefab;
    [SerializeField] private Transform[] panelPositions;
    [SerializeField] private List<PowerUp> powerUps;

    [Space]
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private float fadeDuration = 0.2f;

    private Tweener tweenAnimation;
    private SuperPlayer superPlayer;

    private List<PowerUpPanel> powerUpPanels = new List<PowerUpPanel>();
    private List<PowerUp> usedPowerUps = new List<PowerUp>();
    private PowerUp tempPowerUp;

    private void Awake()
    {
        usedPowerUps.Clear();

        for (int i = 0; i < panelPositions.Length; i++)
        {
            var panel = Instantiate(panelPrefab, panelPositions[i]);

            powerUpPanels.Add(panel);

            panel.FillPanel(SelectPowerUp());
        }
    }

    private void Start()
    {
        superPlayer = FindObjectOfType<SuperPlayer>();
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
        superPlayer.AddMaxLives();
        Debug.Log("AddMaxLives");
    }

    public void FillHp()
    {
        superPlayer.FillHp();
        Debug.Log("FillHp");
    }

    public void AddSpeed()
    {
        superPlayer.AddSpeed();
        Debug.Log("AddMaxSpeed");
    }

    public void EnableRun()
    {
        superPlayer.EnableRun();
        Debug.Log("EnableRun");
    }

    public void AddRunningSpeedMultiplier()
    {
        superPlayer.AddRunningSpeedMultiplier();
        Debug.Log("AddRunningSpeedMultiplier");
    }

    public void AddJumpForce()
    {
        superPlayer.AddJumpForce();
        Debug.Log("AddJumpForce");
    }

    public void AddAdditionalJump()
    {
        superPlayer.AddAdditionalJumps();
        Debug.Log("AddAdditionalJumps");
    }

    public void EnableExtraJump()
    {
        superPlayer.EnableExtraJumps();
        Debug.Log("EnableExtraJumps");
    }

    public void AddAttackValue()
    {
        superPlayer.AddAttackValue();
        Debug.Log("AddAttackValue");
    }

    public void AddAdditionalStones()
    {
        superPlayer.AddAdditionalStones();
        Debug.Log("AddAdditionalStones");
    }

    public void EnableRangeAttack()
    {
        superPlayer.EnableRangeAttack();
        Debug.Log("EnableRangeAttack");
    }

    public void Hide()
    {
        tweenAnimation?.Kill();
        canvasGroup.DOFade(0, fadeDuration).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
    }
}
