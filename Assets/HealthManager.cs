using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private SuperPlayer player;
    private int numberOfHearts;
    private int health;

    private void OnEnable()
    {
        SuperPlayer.SuperPlayer_OnDamaged += UpdateHearts;
    }

    private void OnDisable()
    {
        SuperPlayer.SuperPlayer_OnDamaged -= UpdateHearts;
    }

    private void Start()
    {
        player = FindObjectOfType<SuperPlayer>();
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        numberOfHearts = player.MAXHealth;
        health = player.CurrentHealth;

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
}
