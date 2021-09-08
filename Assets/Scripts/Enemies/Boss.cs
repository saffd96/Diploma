using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : BaseEnemy
{
    private bool isDead;
    public bool IsDead => isDead;
    protected override void Die()
    {
        base.Die();
        isDead = true;
    }
}
