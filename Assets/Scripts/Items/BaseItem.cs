using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    [SerializeField] private GameObject activateVfx;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out SuperPlayer player)) return;

        if (activateVfx != null)
        {
            Instantiate(activateVfx, transform.position, Quaternion.identity);
        }

        ApplyEffect(player);
        AudioManager.Instance.PLaySfx(SfxType.PickUp);
        Destroy(gameObject);
    }

    protected abstract void ApplyEffect(SuperPlayer player);
}
