using UnityEngine;

public class BaseEnemy : DamageableObject
{
    [SerializeField] private SpriteRenderer mapSprite;

    protected Collider2D Coll2D;
    protected Rigidbody2D Rb2D;

    protected override void Awake()
    {
        base.Awake();

        Coll2D = GetComponent<Collider2D>();

        if (Coll2D == null)
        {
            Coll2D = GetComponentInChildren<Collider2D>();
        }

        Rb2D = GetComponent<Rigidbody2D>();
    }

    protected override void Die()
    {
        base.Die();
        mapSprite.enabled = false;
    }
}
