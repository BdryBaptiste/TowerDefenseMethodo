using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : TowerEffect
{
    public SlowEffect(float magnitude, int duration) : base("Slow", magnitude, duration) { }

    public override void ApplyEffect(Enemy enemy)
    {
        enemy.ApplyEffect(this);
        Debug.Log($"Applied slow effect to {enemy.name}, reducing speed by {Magnitude} for {Duration} seconds.");
    }
}