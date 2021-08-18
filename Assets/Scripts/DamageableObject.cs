using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    protected int health;

    public virtual void ApplyDamage(int amount)
    {
        health -= amount;
    }
}
