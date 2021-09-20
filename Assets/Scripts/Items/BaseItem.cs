using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out SuperPlayer player)) return;
        
        ApplyEffect(player);
        Destroy(gameObject);
    }

    protected abstract void ApplyEffect(SuperPlayer player);
}
