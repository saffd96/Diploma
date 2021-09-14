using UnityEngine;

public class MovingShadow : MonoBehaviour
{
    [Header("Shadow Settings")]
    [SerializeField] private Transform shadowTransform;
    [SerializeField] private float shadowShowRange = 3f;

    private RaycastHit2D hit;
    private bool isShadowEnabled;

    private void Update()
    {
        MoveShadow();
    }

    private void MoveShadow()
    {
        var position = transform.position;

        hit = Physics2D.Raycast(position, Vector2.down, shadowShowRange, LayerMask.GetMask(Layers.Ground));

        isShadowEnabled = hit.collider != null;

        if (isShadowEnabled)
        {
            shadowTransform.position = hit.point;
        }

        shadowTransform.gameObject.SetActive(isShadowEnabled);
    }
}
