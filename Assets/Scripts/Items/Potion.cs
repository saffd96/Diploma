public class Potion: BaseItem
{
    protected override void ApplyEffect(SuperPlayer player)
    {
        player.PotionItem();
    }
}
