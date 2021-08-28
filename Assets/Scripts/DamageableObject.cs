using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    [SerializeField] protected int MAXHealth;

    private int currentHealth;

    protected void Awake()
    {
        currentHealth = MAXHealth;
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
