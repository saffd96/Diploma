using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : BaseEnemy
{
    public bool IsDead { get; private set; }

    protected override void Die()
    {
        IsDead = true;
       // base.Die();
    }
}
