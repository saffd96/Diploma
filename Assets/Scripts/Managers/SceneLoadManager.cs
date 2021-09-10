using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text progressText;
    private float waitToLoad = 2f;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == SceneNamesConstants.LoadingScene)
        {
            LoadScene(GameHandler.LevelsCompleted == GameHandler.NeedCastleScenesToPass
                    ? SceneNamesConstants.BossLevel
                    : SceneNamesConstants.CastleLevel);
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
            var progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (slider != null)
            {
                slider.value = progress;
            }

            if (progressText != null)
            {
                progressText.text = progress * 100f + "%";
            }

            yield return null;
        }
    }
}
