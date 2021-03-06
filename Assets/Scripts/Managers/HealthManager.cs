using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Text heartsText;
    [SerializeField] private Image bloodOnScreen;

    [Header("Boss")]
    [SerializeField] private GameObject bossHp;

    [SerializeField] private Slider slider;
    private Player player;
    private Boss boss;

    private int numberOfHearts;
    private int health;

    private void OnEnable()
    {
        Player.OnPlayerHpChanged += UpdateHearts;
        Boss.OnBossHpChanged += UpdateBossHp;
        Boss.OnEnterChaseZone += SetupBossHp;
    }

    private void OnDisable()
    {
        Player.OnPlayerHpChanged -= UpdateHearts;
        Boss.OnBossHpChanged -= UpdateBossHp;
        Boss.OnEnterChaseZone -= SetupBossHp;
    }

    private void Start()
    {
        bossHp.SetActive(false);

        player = FindObjectOfType<Player>();
        UpdateHearts();
    }

    private void SetupBossHp()
    {
        slider = bossHp.GetComponentInChildren<Slider>();
        boss = FindObjectOfType<Boss>();
        bossHp.SetActive(true);

        if (slider == null) return;

        slider.maxValue = boss.MAXHealth;
        UpdateBossHp();
    }

    private void UpdateHearts()
    {
        numberOfHearts = player.MAXHealth;
        health = player.CurrentHealth;

        bloodOnScreen.enabled = health == 1;

        if (health <= hearts.Length)
        {
            heartsText.enabled = false;

            if (health > numberOfHearts)
            {
                health = numberOfHearts;
            }

            for (var i = 0; i < hearts.Length; i++)
            {
                hearts[i].sprite = i < health ? fullHeart : emptyHeart;
                hearts[i].enabled = i < numberOfHearts;
            }
        }
        else
        {
            for (var i = 1; i < hearts.Length; i++)
            {
                hearts[i].enabled = false;
            }

            hearts[0].sprite = fullHeart;
            heartsText.enabled = true;
            heartsText.text = $"x{health.ToString()}";
        }
    }

    private void UpdateBossHp()
    {
        slider.value = boss.CurrentHealth;
    }
}
