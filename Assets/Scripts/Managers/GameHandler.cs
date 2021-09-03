using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    
    public static Vector2 StartPosition;

    private static int levelsCompleted = 0;
    public static int NeedCastleScenesToPass { get; } = 1;
    private bool IsPaused { get; set; }
    private bool IsMapActive { get; set; }
    private bool IsBossDead { get; set; }


    public static int LevelsCompleted => levelsCompleted;

    private void Update()
    {
        CheckPauseToggle();

        ChechMapToggle();

        CheckBossDead();
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

    private void MapToggle()
    {
        IsMapActive = !IsMapActive;
        uiManager.MapToggle(IsMapActive);
    }
    
    

    private void CheckPauseToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
        }
    }

    private void ChechMapToggle()
    {
        if (Input.GetKeyDown(KeyCode.M) && !IsPaused)
        {
            MapToggle();
        }
    }

    private void CheckBossDead()
    {
        if (IsBossDead)
        {
            SceneLoadManager.LoadScene(SceneNamesConstants.EndScene);
        }
    }
}
