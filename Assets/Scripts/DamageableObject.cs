using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    [SerializeField] protected int maxHealth;

    public bool IsInvulnerable { get; protected set; }

    public int MAXHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }
    public int CurrentHealth { get; private set; }

    protected virtual void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public virtual void ApplyDamage(int amount)
    {
        if (IsInvulnerable) return;

        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            Debug.Log("isDead");
            Die();
        }
    }

    protected virtual void Die()
    {
      //  Destroy(gameObject);
    }
}
