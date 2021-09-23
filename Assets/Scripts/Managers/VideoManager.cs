using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private SceneLoadManager sceneLoadManager;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AnimatedPanel animatedPanel;

    private void OnEnable()
    {
        animatedPanel.OnCompleteAnimation += LoadSceneAfterAnimation;
    }

    private void OnDisable()
    {
        animatedPanel.OnCompleteAnimation -= LoadSceneAfterAnimation;
    }

    private void Update()
    {
        if (!videoPlayer.isPlaying)
        {
            animatedPanel.PlayAnimation();
        }
    }

    private void LoadSceneAfterAnimation()
    {
        sceneLoadManager.LoadScene(SceneNamesConstants.MenuScene);
    }
}
