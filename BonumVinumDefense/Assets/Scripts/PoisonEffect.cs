using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : EnemyEffect
{
    public float DamagePerTick { get; private set; }
    public float TickInterval { get; private set; }
    public float NextTickTime { get; set; }

    public PoisonEffect(float damagePerTick, float tickInterval, float duration) : base(duration)
    {
        DamagePerTick = damagePerTick;
        TickInterval = tickInterval;
        NextTickTime = Time.time + TickInterval;
    }
}
