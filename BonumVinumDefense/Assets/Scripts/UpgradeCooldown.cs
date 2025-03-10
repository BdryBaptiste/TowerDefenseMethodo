using UnityEngine;

public class UpgradeCooldown : TowerUpgradeDecorator
{
    private float cooldownReduction;

    public UpgradeCooldown(Tower tower, float cooldownReduction) : base(tower)
    {
        this.cooldownReduction = cooldownReduction;
    }

    public override float Cooldown => Mathf.Max(0.1f, baseTower.Cooldown - cooldownReduction);
}
