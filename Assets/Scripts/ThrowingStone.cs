using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ThrowingStone : MonoBehaviour
{
    private int damage = 1;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }

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
        }
        
       
        
        //
        // if (!other.collider.IsTouchingLayers(LayerMask.GetMask(Layers.Enemy))) return;
        //
        //
        //
        // if (other.gameObject.TryGetComponent(out DamageableObject damageableObject))
        // {
        //     Debug.Log("VAR");
        //     damageableObject.ApplyDamage(damage);
        //     Destroy(gameObject);
        // }
        
    }
}
