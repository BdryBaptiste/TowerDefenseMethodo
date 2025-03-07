public abstract class TowerEffect
{
    public string EffectType { get; protected set; }
    public float Magnitude { get; protected set; }
    public int Duration { get; protected set; }

    public TowerEffect(string type, float magnitude, int duration)
    {
        EffectType = type;
        Magnitude = magnitude;
        Duration = duration;
    }
}