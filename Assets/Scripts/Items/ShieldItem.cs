using PlayerComponents;

public class ShieldItem : BaseItem
{
    protected override void ApplyEffect(PlayerItemEffects playerItemEffects)
    {
        playerItemEffects.AddShieldItem();
    }
}
