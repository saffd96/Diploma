using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    [SerializeField] private SceneLoadManager sceneLoadManager;
    [SerializeField] private PowerUpManager powerUpManager;

    public static Vector2 StartPosition;
    public static GameObject Player;

    private Boss boss;

    public static int NeedCastleScenesToPass { get; private set; } = 1;
    private bool IsPaused { get; set; }
    private bool IsMapActive { get; set; }

    private bool isBossDead;
    public static bool IsPowerUpSelected { get; private set; }
    public static int LevelsCompleted { get; private set; }

    private void OnEnable()
    {
        ExitLvl.OnExitLvlCollision += CompleteLvl;
        Boss.OnBossDeath += OnBossDeath;
        SuperPlayer.OnSuperPlayerDeath += OnPLayerDeath;
    }

    private void OnDisable()
    {
        ExitLvl.OnExitLvlCollision -= CompleteLvl;
        Boss.OnBossDeath -= OnBossDeath;
        SuperPlayer.OnSuperPlayerDeath -= OnPLayerDeath;
    }

    private void Awake()
    {
        IsPowerUpSelected = false;

        if (SceneManager.GetActiveScene().name == SceneNamesConstants.CastleLevel)
        {
            Time.timeScale = 0f;
        }
    }

    private void Start()
    {
        if (powerUpManager != null)
        {
            powerUpManager.gameObject.SetActive(true);
        }

        boss = FindObjectOfType<Boss>();
    }

    private void Update()
    {

        
        CheckPauseToggle();

        CheckMapToggle();
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
        uiManager.PowerUpHide();
    }

    private void CompleteLvl()
    {
        LevelsCompleted++;
        
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

    private void OnBossDeath()
    {
        //DoTweenShade after load scene;
        sceneLoadManager.LoadScene(SceneNamesConstants.EndScene);
    }

    private void OnPLayerDeath()
    {
        uiManager.ShowDeathScreen();
    }

    public static bool IsCastlePassed()
    {
        return LevelsCompleted >= NeedCastleScenesToPass;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
