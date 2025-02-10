using System.Collections.Generic;
using UnityEngine;

public class ActiveEffect
{
    public TowerEffect Effect { get; private set; }
    public float StartTime { get; private set; }

    public ActiveEffect(TowerEffect effect, float startTime)
    {
        Effect = effect;
        StartTime = startTime;
    }
}
