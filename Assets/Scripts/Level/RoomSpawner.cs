using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private LevelGeneration levelGeneration;
    [SerializeField] private GameObject block;

    private void Update()
    {
        if (!levelGeneration.StopGeneration) return;

        var roomDetection = Physics2D.OverlapCircle(transform.position, 1, LayerMask.GetMask(Layers.Room));

        if (roomDetection==null)
        {
            Instantiate(block, transform.position, Quaternion.identity, levelGeneration.LevelTransform );
            Destroy(gameObject);
        }

    }
}
