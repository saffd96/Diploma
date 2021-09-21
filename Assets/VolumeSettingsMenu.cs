using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsMenu : MonoBehaviour
{
    [SerializeField] private float volumeMultiplier = 100f;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private Text musicText;
    [SerializeField] private Text sfxText;
    
    private float musicValue, sfxValue;

    private void Start()
    {
        musicSlider.minValue = 0;
        musicSlider.maxValue = volumeMultiplier;

        sfxSlider.minValue = 0;
        sfxSlider.maxValue = volumeMultiplier;

        AudioManager.Instance.LoadValues();
        SetSliderValue();
    }

    private void Update()
    {
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        musicValue = musicSlider.value / volumeMultiplier;
        sfxValue = sfxSlider.value / volumeMultiplier;
        
        AudioManager.Instance.SetMusicVolume(musicValue);
        AudioManager.Instance.SetSfxVolume(sfxValue);

        musicText.text = $"{GetMusicVolume():0}";
        sfxText.text = $"{GetSfxVolume():0}";
    }

    private void SetSliderValue()
    {
        musicSlider.value = GetMusicVolume();

        sfxSlider.value = GetSfxVolume();
    }
    
    private float GetSfxVolume()
    {
        return AudioManager.Instance.SfxVolume * volumeMultiplier;
    }

    private float GetMusicVolume()
    {
        return AudioManager.Instance.MusicVolume * volumeMultiplier;
    }
}
