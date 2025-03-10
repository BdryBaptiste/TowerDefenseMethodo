public class UpgradeRange : TowerUpgradeDecorator
{
    private float extraRange;

    public UpgradeRange(Tower tower, float extraRange) : base(tower)
    {
        this.extraRange = extraRange;
    }

    public override float Range => baseTower.Range + extraRange;
}
