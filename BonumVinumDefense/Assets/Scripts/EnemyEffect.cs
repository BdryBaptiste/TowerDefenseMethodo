public abstract class EnemyEffect
{
    public float Duration { get; protected set; }

    public EnemyEffect(float duration)
    {
        Duration = duration;
    }
}
