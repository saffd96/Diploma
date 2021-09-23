using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    [SerializeField] private GameObject deathVfx;
    [SerializeField] private GameObject hitVfx;

    protected bool IsDead;
    protected Animator Animator;

    public bool IsInvulnerable { get; protected set; }
    public int MAXHealth => maxHealth;
    public int CurrentHealth { get; protected set; }

    protected virtual void Awake()
    {
        Animator = GetComponentInChildren<Animator>();

        CurrentHealth = maxHealth;
    }

    public virtual void ApplyDamage(int amount)
    {
        if (IsInvulnerable || IsDead) return;

        Animator.SetTrigger(AnimationTriggerNames.Hurt);

        if (hitVfx != null)
        {
            Instantiate(hitVfx, transform.position, Quaternion.identity, transform);
        }

        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (deathVfx != null)
        {
            Instantiate(deathVfx, transform.position, Quaternion.identity);
        }

        IsDead = true;
    }
}
