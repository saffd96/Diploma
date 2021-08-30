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

    private void Start()
    {
        player = FindObjectOfType<SuperPlayer>();
        numberOfHearts = player.MAXHealth;
        health  = player.CurrentHealth;
        GenerateHearts();
    }

    private void Update()
    {
        numberOfHearts = player.MAXHealth;
        health  = player.CurrentHealth;
        GenerateHearts();
        
    }

    private void GenerateHearts()
    {
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
