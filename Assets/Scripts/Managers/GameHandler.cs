using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    
    public static Vector2 StartPosition;

    private static int levelsCompleted = 0;
    private bool IsPaused { get; set; }
    private bool IsMapActive { get; set; }

    public static int LevelsCompleted => levelsCompleted;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
        }

        if (Input.GetKeyDown(KeyCode.M)&&!IsPaused)
        {
            MapToggle();
        }
    }

    public static void CompleteLvl()
    {
        levelsCompleted++;
    }

    public void PauseToggle()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
        uiManager.PauseToggle(IsPaused);
    }
    
    public void MapToggle()
    {
        IsMapActive = !IsMapActive;
        uiManager.MapToggle(IsMapActive);
    }
}
