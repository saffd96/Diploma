using UnityEngine;
using PlayerComponents;

public class InvulnerablePotion : BaseItem
{
    [SerializeField] private int seconds = 5;

    protected override void ApplyEffect(PlayerItemEffects playerItemEffects)
    {
        playerItemEffects.InvulnerableItem(seconds);
    }
}
