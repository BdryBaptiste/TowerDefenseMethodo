public class SlowEffect : EnemyEffect
{
    public float SlowPercentage { get; private set; }

    public SlowEffect(float slowPercentage, float duration) : base(duration)
    {
        SlowPercentage = slowPercentage;
    }
}
