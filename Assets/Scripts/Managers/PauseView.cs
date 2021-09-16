using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration;
    [SerializeField] private Button[] buttons;

    [Space]
    [SerializeField] private Text levelsPassed;
    [Header("Settings")]
    [SerializeField] private float volumeMultiplier = 100f;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private Text musicText;
    [SerializeField] private Text sfxText;

    private Tweener tweenAnimation;

    private void Start()
    {
        canvasGroup.alpha = 0;
    }

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

        foreach (var button in buttons)
        {
            if (!button.enabled)
            {
                button.enabled = true;
            }
        }

        tweenAnimation?.Kill();
        canvasGroup.DOFade(1, fadeDuration).SetUpdate(true);
    }

    public void Hide()
    {
        foreach (var button in buttons)
        {
            if (button.enabled)
            {
                button.enabled = false;
            }
        }

        tweenAnimation?.Kill();
        canvasGroup.DOFade(0, fadeDuration).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
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
