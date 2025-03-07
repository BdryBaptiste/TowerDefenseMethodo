using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffectDecorator : TowerDecorator
{
    private float damagePerTick;
    private float tickInterval;
    private float duration;

    public PoisonEffectDecorator(ITowerStrategy strategy, float damage, float interval, float duration) : base(strategy)
    {
        this.damagePerTick = damage;
        this.tickInterval = interval;
        this.duration = duration;
    }

    public override void Execute(Tower tower)
    {
        base.Execute(tower);
        ApplyPoisonEffect(tower);
    }

    private void ApplyPoisonEffect(Tower tower)
    {
        Collider[] hitColliders = Physics.OverlapSphere(tower.firePoint.position, tower.Range);
        foreach (Collider hit in hitColliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ApplyEffect(new PoisonEffect(damagePerTick, tickInterval, duration));
            }
        }
    }
}