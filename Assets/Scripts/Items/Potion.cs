using UnityEngine;
using PlayerComponents;

public class Potion : BaseItem
{
    [SerializeField] private int hpAmount = 1;

    protected override void ApplyEffect(PlayerItemEffects playerItemEffects)
    {
        playerItemEffects.PotionItem(hpAmount);
    }
}
