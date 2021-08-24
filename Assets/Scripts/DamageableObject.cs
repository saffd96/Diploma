using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    protected int Health;

    public virtual void ApplyDamage(int amount)
    {
        Health -= amount;
    }
}
