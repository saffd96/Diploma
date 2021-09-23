using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    [SerializeField] private Transform exit;

    [SerializeField] private GameObject buttonSprite;

    private GameObject player;
    private bool isEnter;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(exit.position, 0.1f);
    }

    private void Awake()
    {
        HideButtonSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPLayerEnter(other))
        {
            player = other.gameObject;
            isEnter = true;
            ShowButtonSprite();
        }
    }

    private void Update()
    {
        if (isEnter && Input.GetKeyDown(KeyCode.W))
        {
            player.transform.position = exit.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsPLayerEnter(other))
        {
            isEnter = false;
            player = null;
            HideButtonSprite();
        }
    }

    private bool IsPLayerEnter(Collider2D collider)
    {
        return collider.CompareTag(Tags.Player);
    }

    private void ShowButtonSprite()
    {
        buttonSprite.SetActive(true);
    }

    private void HideButtonSprite()
    {
        buttonSprite.SetActive(false);
    }
}
