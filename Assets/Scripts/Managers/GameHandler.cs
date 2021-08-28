using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    public static Vector2 StartPosition;

    private static int levelsCompleted = 0;
    private bool IsPaused { get; set; }

    public static int LevelsCompleted => levelsCompleted;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
        }
    }

    public static void CompleteLvl()
    {
        levelsCompleted++;
    }

    private void PauseToggle()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
        uiManager.PauseToggle(IsPaused);
    }
}
