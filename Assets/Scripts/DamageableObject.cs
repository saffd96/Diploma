using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected bool isDead;
    protected Animator Animator;

    public bool IsInvulnerable { get; protected set; }
    public bool IsDead { get; protected set; }

    public int MAXHealth => maxHealth;
    public int CurrentHealth { get; protected set; }

    protected virtual void Awake()
    {
        Animator = GetComponentInChildren<Animator>();

        if (GameHandler.LevelsCompleted == 0)
        {
            CurrentHealth = maxHealth;
        }
    }

    public virtual void ApplyDamage(int amount)
    {
        if (IsInvulnerable || isDead) return;

        Animator.SetTrigger(AnimationTriggerNames.Hurt);
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            Debug.Log("isDead");
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
    }
}
