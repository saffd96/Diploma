using UnityEngine;
using UnityEngine.UI;
using PlayerComponents;

public class StoneView : MonoBehaviour
{
    [SerializeField] private Text stonesText;
    [SerializeField] private CanvasGroup canvasGroup;

    public CanvasGroup CanvasGroup => canvasGroup;

    private PlayerAttack playerAttack;
    private int stones;

    private void OnEnable()
    {
        PlayerAttack.OnStonesChanged += UpdateStones;
    }

    private void OnDisable()
    {
        PlayerAttack.OnStonesChanged -= UpdateStones;
    }

    private void Start()
    {
        playerAttack = FindObjectOfType<PlayerAttack>();

        UpdateStones();

        if (playerAttack.CurrentStones == 0)
        {
            canvasGroup.alpha = 0f;
        }
    }

    private void UpdateStones()
    {
        stones = playerAttack.CurrentStones;

        if (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha = 1f;
        }

        stonesText.text = $"x{stones.ToString()}";
    }
}
