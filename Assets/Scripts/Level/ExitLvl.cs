using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLvl : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        GameHandler.CompleteLvl();
        
        SceneManager.LoadScene(GameHandler.LevelsCompleted != GameHandler.NeedCastleScenesToPass
                ? SceneNamesConstants.CastleLevel
                : SceneNamesConstants.BossLevel);
    }
}
