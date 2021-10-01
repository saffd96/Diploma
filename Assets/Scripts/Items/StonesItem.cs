using UnityEngine;
using PlayerComponents;

public class StonesItem : BaseItem
{
    [SerializeField] private int stonesAmount = 2;

    protected override void ApplyEffect(PlayerItemEffects playerItemEffects)
    {
        playerItemEffects.AddStonesItem(stonesAmount);
    }
}
