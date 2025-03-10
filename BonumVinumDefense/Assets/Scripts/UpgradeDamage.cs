public class UpgradeDamage : TowerUpgradeDecorator
{
    private int additionalDamage;

    public UpgradeDamage(Tower tower, int extraDamage) : base(tower)
    {
        this.additionalDamage = extraDamage;
    }

    public override int Damage => baseTower.Damage + additionalDamage;
}
