using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class BaseEnemy : DamageableObject
{
   [SerializeField] private SpriteRenderer mapSprite;
   
   
   protected Animator animator;
   private Collider2D collider2D;
   private Rigidbody2D rigidbody2D;

   protected override void Awake()
   {
      base.Awake();
      animator = GetComponentInChildren<Animator>();
      collider2D = GetComponent<Collider2D>();
      rigidbody2D = GetComponent<Rigidbody2D>();
   }

   protected override void Die()
   {
      base.Die();
      mapSprite.enabled = false;
   }
}
