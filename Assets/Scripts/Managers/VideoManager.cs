using System;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private SceneLoadManager sceneLoadManager;
    [SerializeField] private VideoPlayer videoPlayer;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (!videoPlayer.isPlaying)
        {
            sceneLoadManager.LoadScene(SceneNamesConstants.MenuScene);
            Destroy(audioManager);
        }
    }
}
