using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private float waitToLoad = 2f;
    
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == SceneNamesConstants.LoadingScene)
        {
            StartCoroutine(WaitForLoading());
        }
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator WaitForLoading()
    {
        yield return new WaitForSeconds(waitToLoad);

        SceneManager.LoadScene(SceneNamesConstants.CastleLevel);
    }
}
