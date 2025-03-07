public abstract class TowerDecorator : ITowerStrategy
{
    protected ITowerStrategy wrappedStrategy;

    public TowerDecorator(ITowerStrategy strategy)
    {
        this.wrappedStrategy = strategy;
    }

    public virtual void Execute(Tower tower)
    {
        wrappedStrategy.Execute(tower);
    }
}

