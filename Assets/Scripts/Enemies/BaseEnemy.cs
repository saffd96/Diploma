using UnityEngine;

public class BaseEnemy : DamageableObject
{
   [SerializeField] private SpriteRenderer mapSprite;

   protected Collider2D coll2D;
   protected Rigidbody2D rb2D;

   protected override void Awake()
   {
      base.Awake();

      coll2D = GetComponent<Collider2D>();

      if (coll2D == null)
      {
         coll2D = GetComponentInChildren<Collider2D>();
      }
      
      rb2D = GetComponent<Rigidbody2D>();
   }

   protected override void Die()
   {
      base.Die();
      mapSprite.enabled = false;
   }
}
