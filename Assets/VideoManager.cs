using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private SceneLoadManager sceneLoadManager;

    [SerializeField] private VideoPlayer videoPlayer;


    private void Update()
    {
        if (!videoPlayer.isPlaying)
        {
            sceneLoadManager.LoadScene(SceneNamesConstants.MenuScene);
        }
    }
}
