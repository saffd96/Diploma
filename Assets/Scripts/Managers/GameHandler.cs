using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    [SerializeField] private SceneLoadManager sceneLoadManager;
    [SerializeField] private PowerUpManager powerUpManager;
    [SerializeField] private GameObject onClickVfx;
    [SerializeField] private AnimatedPanel exitPanel;
    

    public static Vector2 StartPosition;
    public static GameObject Player;
    
    private static int NeedCastleScenesToPass { get; set; } = 1;
    private bool IsPaused { get; set; }
    private bool IsMapActive { get; set; }
    private static bool IsPowerUpSelected { get; set; }
    public static int LevelsCompleted { get; private set; }

    private void OnEnable()
    {
        ExitLvl.OnExitLvlCollision += CompleteLvl;
        SuperPlayer.OnSuperPlayerDeath += OnPLayerDeath;
        BossExitLvl.OnBossExitDoorCollision += ResetGame;
        exitPanel.OnCompleteAnimation += ExitGame;
        exitPanel.OnCompleteReverseAnimation += DisableExitPanel;
    }

    private void OnDisable()
    {
        ExitLvl.OnExitLvlCollision -= CompleteLvl;
        SuperPlayer.OnSuperPlayerDeath -= OnPLayerDeath;
        BossExitLvl.OnBossExitDoorCollision -= ResetGame;
        exitPanel.OnCompleteAnimation -= ExitGame;
        exitPanel.OnCompleteReverseAnimation -= DisableExitPanel;

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
        exitPanel.gameObject.SetActive(true);
        exitPanel.PlayReverseAnimation();
        if (powerUpManager != null)
        {
            powerUpManager.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        CheckPauseToggle();

        CheckMapToggle();
    }

    public void PauseToggle()
    {
        if (IsPowerUpSelected)
        {
            IsPaused = !IsPaused;
            Time.timeScale = IsPaused ? 0f : 1f;
            uiManager.PauseToggle(IsPaused);
        }
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

    public static bool IsCastlePassed()
    {
        return LevelsCompleted >= NeedCastleScenesToPass;
    }

    public void CreateOnClickVfx()
    {
        if (onClickVfx != null)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(onClickVfx, mousePos, quaternion.identity, transform);
        }
    }

    public void CreateOnHoverSound()
    {
        AudioManager.Instance.PlayButtonOnHoverSfx();
    }

    public void CreateOnUpSound()
    {
        AudioManager.Instance.PlayButtonOnClickSfx();
    }

    
    private void ResetGame()
    {
        LevelsCompleted = 0;
    }

    private void ExitGame()
    {
        Application.Quit();
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
        if (Input.GetKeyDown(KeyCode.Escape) && uiManager != null)
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

    private void OnPLayerDeath()
    {
        LevelsCompleted = 0;
        uiManager.ShowDeathScreen();
    }

    public void PlayExitAnimation()
    {
        exitPanel.gameObject.SetActive(true);
        exitPanel.PlayAnimation();
    }
    
    private void DisableExitPanel()
    {
        exitPanel.gameObject.SetActive(false);
    }
}
