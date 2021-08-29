using System;
using UnityEngine;

public class GameAudioSource : MonoBehaviour
{
    private AudioSource audioSource;
    private SfxInfo sfxInfo;

    private Action<GameAudioSource> onKill;

    private const bool Loop = false;
    private const bool PlayOnAwake = true;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = Loop;
        audioSource.playOnAwake = PlayOnAwake;
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            RemoveAudioSource();
        }
    }

    public void Setup(SfxInfo sfxInfo, float globalSfxVolume)
    {
        this.sfxInfo = sfxInfo;

        audioSource.clip = sfxInfo.Clip;
        SetVolume(globalSfxVolume);
    }

    public void PlayOneShot(SfxInfo sfxInfo)
    {
        audioSource.PlayOneShot(sfxInfo.Clip);
    }

    public void SetVolume(float globalSfxVolume)
    {
        audioSource.volume = sfxInfo.Volume * globalSfxVolume;
    }

    public GameAudioSource OnKill(Action<GameAudioSource> onKill)
    {
        this.onKill = onKill;

        return this;
    }

    private void RemoveAudioSource()
    {
        onKill?.Invoke(this);

        Destroy(audioSource);
        Destroy(this);
    }
}
