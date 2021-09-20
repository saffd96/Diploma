public class ShieldItem : BaseItem
{
    protected override void ApplyEffect(SuperPlayer superPlayer)
    {
        superPlayer.AddShield();
    }
}
