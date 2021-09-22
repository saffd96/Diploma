public class StonesItem : BaseItem
{
    protected override void ApplyEffect(SuperPlayer player)
    {
        player.AddStonesItem();
    }
}
