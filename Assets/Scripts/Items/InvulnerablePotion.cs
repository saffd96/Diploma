public class InvulnerablePotion : BaseItem
{
    protected override void ApplyEffect(SuperPlayer player)
    {
        player.InvulnerableItem();
    }
}
