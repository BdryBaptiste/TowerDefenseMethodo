using System.Collections.Generic;
using UnityEngine;

public class BurnEffect : EnemyEffect
{
    public float DamagePerTick { get; private set; }
    public float TickInterval { get; private set; }
    public float NextTickTime { get; set; }

    public BurnEffect(float damagePerTick, float tickInterval, float duration) : base(duration)
    {
        DamagePerTick = damagePerTick;
        TickInterval = tickInterval;
        NextTickTime = Time.time + TickInterval;
    }
}
