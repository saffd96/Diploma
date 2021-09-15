using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "PowerUp")]
public class PowerUp : ScriptableObject
{
    public string PowerUpName;
    [TextArea(minLines: 3, maxLines: 5)]
    public string Description;

    public Sprite Image;

    public Action ActionWithPlayer;

    public enum Action
    {
        AddLives,
        FillHp,
        AddSpeed,
        EnableRun,
        AddRunningSpeedMultiplier,
        AddJumpForce,
        AddAdditionalJumps,
        EnableExtraJumps,
        AddAttackValue,
        EnableRangeAttack,
        AddAdditionalStones
    }
}
