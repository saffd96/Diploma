using UnityEngine;

public class BaseEnemy : DamageableObject
{
   protected Animator animator;

   protected override void Awake()
   {
      base.Awake();
      animator = GetComponentInChildren<Animator>();
   }
}
