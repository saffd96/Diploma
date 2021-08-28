using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private PauseView pauseView;

    public void PauseToggle(bool isActive)
    {
        if (isActive)
        {
            pauseView.Show();
        }
        else
        {
            pauseView.Hide();
        }
    }
}
