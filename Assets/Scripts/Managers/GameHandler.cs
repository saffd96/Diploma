using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    [SerializeField] private SceneLoadManager sceneLoadManager;
    [SerializeField] private PowerUpManager powerUpManager;

    private static int levelsCompleted = 0;

    public static Vector2 StartPosition;
    public static GameObject Player;

    private Boss boss;

    public static int NeedCastleScenesToPass { get; private set; } = 1;
    private bool IsPaused { get; set; }
    private bool IsMapActive { get; set; }

    private bool isBossDead;
    public static bool IsPowerUpSelected { get; private set; }
    public static int LevelsCompleted => levelsCompleted;

    private void OnEnable()
    {
        ExitLvl.OnExitLvlCollision += CompleteLvl;
    }

    private void OnDisable()
    {
        ExitLvl.OnExitLvlCollision -= CompleteLvl;
    }

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == SceneNamesConstants.CastleLevel)
        {
            Time.timeScale = 0f;
        }
    }

    private void Start()
    {
        if (powerUpManager!=null)
        {
            powerUpManager.gameObject.SetActive(true);
        }
        boss = FindObjectOfType<Boss>();
    }

    private void Update()
    {
        CheckPauseToggle();

        CheckMapToggle();

        CheckBossDead();
    }

    public void PauseToggle()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
        uiManager.PauseToggle(IsPaused);
    }

    public void SelectPlayer(GameObject selectedPlayer)
    {
        Player = selectedPlayer;
    }

    public void SetCastleLevel(int amount)
    {
        NeedCastleScenesToPass = amount;
    }

    public void PowerUpPauseToggle()
    {
        IsPowerUpSelected = !IsPowerUpSelected;
        Time.timeScale = !IsPowerUpSelected ? 0f : 1f;
        uiManager.PowerUpToggle(IsPowerUpSelected);
    }

    private void CompleteLvl()
    {
        levelsCompleted++;
        sceneLoadManager.LoadScene(SceneNamesConstants.LoadingScene);
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

        if (boss.IsDead)
        {
            sceneLoadManager.LoadScene(SceneNamesConstants.EndScene);
        }
    }
}
