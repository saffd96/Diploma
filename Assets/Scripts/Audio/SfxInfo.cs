using System;
using UnityEngine;

[Serializable]
public class SfxInfo
{
    private string name;

    public AudioClip Clip;
    public SfxType SfxType;

    [Range(0, 1)]
    public float Volume = 1f;

    public void OnValidate()
    {
        name = SfxType.ToString();
    }
}
