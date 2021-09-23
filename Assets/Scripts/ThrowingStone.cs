using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ThrowingStone : MonoBehaviour
{
    [SerializeField] private GameObject hitVfx;

    private int damage = 1;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out DamageableObject damageableObject))
        {
            other.GetContact(0);

            if (hitVfx != null)
            {
                Instantiate(hitVfx, other.GetContact(0).point, Quaternion.identity);
            }

            damageableObject.ApplyDamage(damage);

            if (!damageableObject.IsInvulnerable)
            {
                Destroy(gameObject);
            }
        }
    }
}
