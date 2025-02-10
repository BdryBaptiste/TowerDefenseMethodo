using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnEffect : TowerEffect
{
    public BurnEffect(float magnitude, int duration) : base("Burn", magnitude, duration) { }

    public override void ApplyEffect(Enemy enemy)
    {
        enemy.ApplyEffect(this);
        Debug.Log($"Applied burn effect to {enemy.name} for {Duration} seconds.");
    }
}
