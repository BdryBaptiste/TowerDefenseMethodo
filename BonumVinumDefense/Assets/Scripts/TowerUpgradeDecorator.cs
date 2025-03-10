using UnityEngine;

public abstract class TowerUpgradeDecorator : Tower
{
    protected Tower baseTower;

    public TowerUpgradeDecorator(Tower tower)
    {
        this.baseTower = tower;
    }

    public override float Range => baseTower.Range;
    public override int Damage => baseTower.Damage;
    public override float Cooldown => baseTower.Cooldown;

    public override void Attack()
    {
        baseTower.Attack(); // Preserve base attack logic
    }
}
