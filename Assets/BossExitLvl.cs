using System;
using UnityEngine;

public class BossExitLvl : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    private SpriteRenderer spriteRenderer;
    private bool isExitEnabled;
    
    public static event Action OnBossExitDoorCollision;
    
    
    private void OnEnable()
    {
        Boss.OnBossDeath += ChangeSprite;
    }

    private void OnDisable()
    {
        Boss.OnBossDeath -= ChangeSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Player) && isExitEnabled)
        {
            OnBossExitDoorCollision?.Invoke();
        }
    }

    private void ChangeSprite()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        isExitEnabled = true;
    }
}
