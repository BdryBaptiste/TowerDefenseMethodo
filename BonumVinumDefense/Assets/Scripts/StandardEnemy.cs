using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemy : EnemyBase
{
    protected override void Start()
    {
        Health = 100;
        Speed = 3.5f;
        Damage = 1;
        Reward = 10;
        base.Start();
    }
}

