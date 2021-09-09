using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    private static int levelsCompleted = 0;
    private Boss boss;
    public static Vector2 StartPosition;

    public static int NeedCastleScenesToPass { get; } = 1;
    private bool IsPaused { get; set; }
    private bool IsMapActive { get; set; }
    private bool IsBossDead { get; set; }

    public static int LevelsCompleted => levelsCompleted;

    private void Awake()
    {
        boss = FindObjectOfType<Boss>();
    }

    private void Update()
    {
        CheckPauseToggle();

        CheckMapToggle();

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

    private void CheckMapToggle()
    {
        if (Input.GetKeyDown(KeyCode.M) && !IsPaused)
        {
            MapToggle();
        }
    }

    private void CheckBossDead()
    {
        if (boss == null)
        {
            return;
        }

        IsBossDead = boss.IsDead;

        if (IsBossDead)
        {
            SceneLoadManager.LoadScene(SceneNamesConstants.EndScene);
        }
    }
}
