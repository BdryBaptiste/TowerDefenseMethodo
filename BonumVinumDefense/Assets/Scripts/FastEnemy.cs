using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemy : EnemyBase
{
    protected override void Start()
    {
        Health = 50;
        Speed = 6f;
        Damage = 1;
        Reward = 15;
        base.Start();
    }
}

