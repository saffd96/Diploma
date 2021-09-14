using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "PowerUp")]
public class PowerUp : ScriptableObject
{
    public string PowerUpName;
    public string Description;

    public Sprite Image;
    
    public Action ActionWithPlayer;
    
    
    public enum Action
    {
        AddLives,
        AddSpeed
    }

}
