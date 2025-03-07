using System.Collections.Generic;
using UnityEngine;

public class ActiveEffect
{
    public EnemyEffect Effect { get; private set; }
    public float StartTime { get; private set; }

    public ActiveEffect(EnemyEffect effect, float startTime)
    {
        Effect = effect;
        StartTime = startTime;
    }

}
