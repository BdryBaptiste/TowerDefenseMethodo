[System.Serializable]
public class PlayerScore
{
    public string Name;
    public int Score;

    public PlayerScore(string name, int score)
    {
        Name = name;
        Score = score;
    }
}