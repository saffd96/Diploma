using System;
using UnityEngine;

[Serializable]
public class MusicInfo
{
    private string name;

    public AudioClip[] Clips;
    public MusicType MusicType;

    public void OnValidate()
    {
        name = MusicType.ToString();
    }
}
