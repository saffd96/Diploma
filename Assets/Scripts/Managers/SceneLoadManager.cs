using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private SceneLoadUi sceneLoadUi;
    [SerializeField] private AnimatedPanel panel;

    private string nextSceneName;

    private void OnEnable()
    {
        BossExitLvl.OnBossExitDoorCollision += LoadEndScene;
        panel.OnCompleteAnimation += LoadSceneAfterAnimation;
    }

    private void OnDisable()
    {
        BossExitLvl.OnBossExitDoorCollision -= LoadEndScene;
        panel.OnCompleteAnimation -= LoadSceneAfterAnimation;
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

    private void FixedUpdate()
    {
        if (sceneLoadUi != null)
        {
            sceneLoadUi.UpdateProgress();
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(SceneLoadDelay(sceneName));
    }

    private void LoadSceneAfterAnimation()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    private void LoadEndScene()
    {
        LoadScene(SceneNamesConstants.EndScene);
    }

    private IEnumerator SceneLoadDelay(string sceneName)
    {
        if (SceneManager.GetActiveScene().name == SceneNamesConstants.LoadingScene)
        {
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return new WaitForSeconds(0);
        }

        if (sceneLoadUi != null)
        {
            sceneLoadUi.SetProgress();
        }

        panel.PlayAnimation();
        nextSceneName = sceneName;
    }
}
