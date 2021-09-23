using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private SceneLoadUi sceneLoadUi;

    private void OnEnable()
    {
        BossExitLvl.OnBossExitDoorCollision += LoadEndScene;
    }

    private void OnDisable()
    {
        BossExitLvl.OnBossExitDoorCollision -= LoadEndScene;
    }

    public static event Action OnSceneCastleLoad;
    
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == SceneNamesConstants.LoadingScene)
        {
            LoadScene(SceneNamesConstants.CastleLevel);

            if (GameHandler.IsCastlePassed())
            {
                OnSceneCastleLoad?.Invoke();
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    private IEnumerator LoadAsynchronously(string sceneName)
    {
        var operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            if (sceneLoadUi != null)
            {
                sceneLoadUi.UpdateProgress(operation);
            }

            yield return null;
        }
    }

    private void LoadEndScene()
    {
        LoadScene(SceneNamesConstants.EndScene);
    }

}
