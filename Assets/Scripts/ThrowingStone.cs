using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ThrowingStone : MonoBehaviour
{
    private int damage = 1;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.collider.IsTouchingLayers(LayerMask.GetMask(Layers.Enemy))) return;

        if (other.gameObject.TryGetComponent(out DamageableObject damageableObject))
        {
            damageableObject.ApplyDamage(damage);
            Destroy(gameObject);
        }
        
    }
}
