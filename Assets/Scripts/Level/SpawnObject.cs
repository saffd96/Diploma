using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;

    private void Awake()
    {
        var objectIndex = Random.Range(0, objects.Length);

        if (objects.Length == 1)
        {
            objectIndex = 0;
        }

        Instantiate(objects[objectIndex], transform.position, Quaternion.identity, transform);
    }
}
