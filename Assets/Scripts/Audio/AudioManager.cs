using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioSettings audioSettings;

    private AudioClip currentClip;

    private readonly List<GameAudioSource> sfxSources = new List<GameAudioSource>();

    public float MusicVolume => bgmSource.volume;
    public float SfxVolume
    {
        get => sfxSource.volume;
        set => sfxSource.volume = value;
    }

    private void Start()
    {
        LoadValues();
        bgmSource.loop = false;
    }

    private void Update()
    {
        PlayMusic(MusicType.Level);
    }

    public void PlayMusic(MusicType musicType)
    {
        if (bgmSource.isPlaying) return;

        do
        {
            currentClip = audioSettings.GetRandomMusic(musicType);

            if (currentClip == null)
            {
                return;
            }
        }

        while (currentClip == bgmSource.clip);

        bgmSource.clip = currentClip;

        bgmSource.Play();
    }

    public void PLaySfx(SfxType sfxType, Transform transform = null)
    {
        var audioClip = GetAudioClip(sfxType);
        var audioSource = CreateAudioSource(transform);

        if (audioClip == null) return;

        if (audioSettings.GetSfxInfo(sfxType) != null)
        {
            SetupAudioSource(audioSource, audioSettings.GetSfxInfo(sfxType));
        }
        else
        {
            Debug.LogError("ERROR");
        }
    }

    private AudioClip GetAudioClip(SfxType sfxType)
    {
        return audioSettings.GetAudioClip(sfxType);
    }

    public void SetMusicVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat(MusicConstants.MusicPrefsKey, volume);
    }

    public void SetSfxVolume(float volume)
    {
        SfxVolume = volume;

        foreach (var gameAudioSource in sfxSources)
        {
            gameAudioSource.SetVolume(volume);
        }

        PlayerPrefs.SetFloat(MusicConstants.SfxPrefsKey, volume);
    }

    private void LoadValues()
    {
        var musicValue = PlayerPrefs.GetFloat(MusicConstants.MusicPrefsKey);
        bgmSource.volume = musicValue;

        var sfxValue = PlayerPrefs.GetFloat(MusicConstants.SfxPrefsKey);
        sfxSource.volume = sfxValue;
    }

    private GameAudioSource CreateAudioSource(Transform transform)
    {
        var transformForAudioSource = transform == null ? this.transform : transform;
        var audioSource = transformForAudioSource.gameObject
               .AddComponent<GameAudioSource>()
               .OnKill(AudioSourceKilled);

        sfxSources.Add(audioSource);

        return audioSource;
    }

    private void AudioSourceKilled(GameAudioSource gameAudioSource)
    {
        sfxSources.Remove(gameAudioSource);
    }

    private void SetupAudioSource(GameAudioSource audioSource, SfxInfo sfxInfo)
    {
        audioSource.Setup(sfxInfo, SfxVolume);
        audioSource.PlayOneShot(sfxInfo);
    }
}
