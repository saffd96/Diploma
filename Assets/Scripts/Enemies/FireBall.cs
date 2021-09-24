using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float damageRadius;
    [SerializeField] private GameObject explodeVfx;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.GetContact(0);

        if (explodeVfx != null)
        {
            Instantiate(explodeVfx, other.GetContact(0).point, Quaternion.identity);
        }

        if (other.gameObject.TryGetComponent(out DamageableObject damageableObject))
        {
            var damageableObjects =
                    Physics2D.OverlapCircleAll(other.GetContact(0).point, damageRadius,
                        LayerMask.GetMask(Layers.Player));

            foreach (var enemy in damageableObjects)
            {
                enemy.GetComponent<DamageableObject>().ApplyDamage(damage);
            }
        }
        AudioManager.Instance.PLaySfx(SfxType.Fireball);
        Destroy(gameObject);
    }
}
