using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AnimatedObject[] animatedObjects;

    [SerializeField] private Button[] buttons;

    [Space]
    [SerializeField] private Text levelsPassed;
    [Header("Settings")]
    [SerializeField] private float volumeMultiplier = 100f;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private Text musicText;
    [SerializeField] private Text sfxText;

    private void Update()
    {
        levelsPassed.text = "Levels passed: " + GameHandler.LevelsCompleted;

        var musicValue = musicSlider.value / volumeMultiplier;
        AudioManager.Instance.SetMusicVolume(musicValue);
        musicText.text = $"{GetMusicVolume():0}";

        var sfxValue = sfxSlider.value / volumeMultiplier;
        AudioManager.Instance.SetSfxVolume(sfxValue);
        sfxText.text = $"{GetSfxVolume():0}";
    }

    public void Show()
    {
        SetVolume();
        gameObject.SetActive(true);

        foreach (var animatedObject in animatedObjects)
        {
            animatedObject.PlayAnimation();
        }

        foreach (var button in buttons)
        {
            if (!button.enabled)
            {
                button.enabled = true;
            }
        }
    }

    public void Hide()
    {
        foreach (var animatedObject in animatedObjects)
        {
            animatedObject.PlayReverseAnimation();
        }

        foreach (var button in buttons)
        {
            if (button.enabled)
            {
                button.enabled = false;
            }
        }
    }

    private float GetSfxVolume()
    {
        return AudioManager.Instance.SfxVolume * volumeMultiplier;
    }

    private float GetMusicVolume()
    {
        return AudioManager.Instance.MusicVolume * volumeMultiplier;
    }

    private void SetVolume()
    {
        musicSlider.minValue = 0;
        musicSlider.maxValue = volumeMultiplier;
        musicSlider.value = GetMusicVolume();

        sfxSlider.minValue = 0;
        sfxSlider.maxValue = volumeMultiplier;
        sfxSlider.value = GetSfxVolume();
    }
}
