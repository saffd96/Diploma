using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private readonly float waitToLoad = 1f;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == SceneNamesConstants.LoadingScene)
        {
            StartCoroutine(WaitForLoading());
        }
    }

    public static void LoadSceneAsync(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    private IEnumerator WaitForLoading()
    {
        yield return new WaitForSeconds(waitToLoad);
        LoadSceneAsync(SceneNamesConstants.CastleLevel);
    }
}
