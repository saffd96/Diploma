using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ThrowingStone : MonoBehaviour
{
    private int damage = 1;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.IsTouchingLayers(LayerMask.GetMask(Layers.Enemy)))
        {

            var damageableObjects =
                    Physics2D.OverlapCircleAll(transform.position, 0.2f,
                        LayerMask.GetMask(Layers.Enemy));
            
            foreach (var enemy in damageableObjects)
            {
                enemy.GetComponent<DamageableObject>().ApplyDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
