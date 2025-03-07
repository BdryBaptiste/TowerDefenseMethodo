using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffectDecorator : TowerDecorator
{
    private float slowAmount;
    private float slowDuration;

    public SlowEffectDecorator(ITowerStrategy strategy, float amount, float duration) : base(strategy)
    {
        this.slowAmount = amount;
        this.slowDuration = duration;
    }

    public override void Execute(Tower tower)
    {
        base.Execute(tower);
        ApplySlowEffect(tower);
    }

    private void ApplySlowEffect(Tower tower)
    {
        if (tower.firePoint != null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(tower.firePoint.position, tower.Range);
            foreach (Collider hit in hitColliders)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ApplyEffect(new SlowEffect(slowAmount, slowDuration));
                }
            }
        }
    }
}

