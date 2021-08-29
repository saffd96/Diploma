using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = nameof(AudioSettings), menuName = "Audio/AudioSettings")]
public class AudioSettings : ScriptableObject
{
    private const string Tag = nameof(AudioSettings);

    [SerializeField] private SfxInfo[] sfx;
    private readonly Dictionary<SfxType, SfxInfo> sfxMap = new Dictionary<SfxType, SfxInfo>();

    [SerializeField] private MusicInfo[] musics;
    private readonly Dictionary<MusicType, MusicInfo> musicMap = new Dictionary<MusicType, MusicInfo>();

    private void OnEnable()
    {
        FillSfxMap();
        FillMusicMap();
    }

    private void OnValidate()
    {
        SfxValidate();
        MusicValidate();
    }

    private void SfxValidate()
    {
        if (sfx == null) return;

        foreach (var sfxInfo in sfx)
        {
            sfxInfo.OnValidate();
        }
    }

    private void MusicValidate()
    {
        if (musics == null) return;

        foreach (var musicInfo in musics)
        {
            musicInfo.OnValidate();
        }
    }

    public AudioClip GetAudioClip(SfxType sfxType)
    {
        return sfxMap.ContainsKey(sfxType) ? sfxMap[sfxType].Clip : null;
    }

    public SfxInfo GetSfxInfo(SfxType sfxType)
    {
        return sfxMap.ContainsKey(sfxType) ? sfxMap[sfxType] : null;
    }

    public AudioClip GetRandomMusic(MusicType musicsType)
    {
        return musicMap.ContainsKey(musicsType)
                ? musicMap[musicsType].Clips[Random.Range(0, musicMap[musicsType].Clips.Length)]
                : null;
    }

    private void FillSfxMap()
    {
        sfxMap.Clear();

        if (sfx == null) return;

        foreach (var sfxInfo in sfx)
        {
            var type = sfxInfo.SfxType;

            if (!sfxMap.ContainsKey(type))
            {
                sfxMap.Add(type, sfxInfo);
            }
            else
            {
                Debug.LogError($"{Tag}, {nameof(FillSfxMap)}: Cannot be more than 1 clip for type '{type}'!");
            }
        }
    }

    private void FillMusicMap()
    {
        musicMap.Clear();

        if (musics == null) return;

        foreach (var musicInfo in musics)
        {
            var type = musicInfo.MusicType;

            if (!musicMap.ContainsKey(type))
            {
                musicMap.Add(type, musicInfo);
            }
            else
            {
                Debug.LogError($"{Tag}, {nameof(FillMusicMap)}: Cannot be more than 1 clip for type '{type}'!");
            }
        }
    }
}
