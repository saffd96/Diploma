using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private SuperPlayer superPlayer;

    private void Start()
    {
        superPlayer = FindObjectOfType<SuperPlayer>();
    }

    public void AddMaxLives()
    {
        superPlayer.AddMaxLives();  
        Debug.Log("AddMaxLives");
    }
    
    public void AddSpeed()
    {
        superPlayer.AddSpeed();
        Debug.Log("AddMaxSpeed");
    }
    
}
