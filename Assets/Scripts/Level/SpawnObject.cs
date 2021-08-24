using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;

    private GameObject instance;

    private void Start()
    {
        var objectIndex = Random.Range(0, objects.Length);

        instance = Instantiate(objects[objectIndex], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
    }
}
