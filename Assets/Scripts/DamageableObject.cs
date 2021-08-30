using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    [SerializeField] protected int maxHealth;

    private int currentHealth;

    public int MAXHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }
    public int CurrentHealth => currentHealth;

    protected void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void ApplyDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Debug.Log("isDead");
            Die();
        }
    }

    protected virtual void Die()
    {
    }
}
