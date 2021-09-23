using UnityEngine;
using UnityEngine.UI;

public class StoneView : MonoBehaviour
{
    [SerializeField] private Text stonesText;
    [SerializeField] private CanvasGroup canvasGroup;

    public CanvasGroup CanvasGroup => canvasGroup;

    private SuperPlayer player;
    private int stones;

    private void OnEnable()
    {
        SuperPlayer.OnSuperPlayerStonesChanged += UpdateStones;
    }

    private void OnDisable()
    {
        SuperPlayer.OnSuperPlayerStonesChanged -= UpdateStones;
    }

    private void Start()
    {
        player = FindObjectOfType<SuperPlayer>();

        UpdateStones();

        if (player.CurrentStones == 0)
        {
            canvasGroup.alpha = 0f;
        }
    }

    private void UpdateStones()
    {
        stones = player.CurrentStones;

        if (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha = 1f;
        }

        stonesText.text = $"x{stones.ToString()}";
    }
}
