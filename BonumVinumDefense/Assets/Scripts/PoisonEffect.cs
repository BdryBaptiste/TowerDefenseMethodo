using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : TowerEffect
{
    public PoisonEffect(float magnitude, int duration) : base("Poison", magnitude, duration) { }

    public override void ApplyEffect(Enemy enemy)
    {
        enemy.ApplyEffect(this);
        Debug.Log($"Applied poison effect to {enemy.name}, dealing {Magnitude} damage over {Duration} seconds.");
    }
}