using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int lives = 20;
    public int currentWave = 0;
    public int gold = 100;

    public Map map;
    public Leaderboard leaderboard;
    public Player player;
    public WaveManager waveManager;

    private void Awake()
    {
        Debug.Log("Game Manager Awake");
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeGame();
    }

    private void InitializeGame()
    {
        if (player == null)
            player = new Player("name");

        player.EarnGold(gold);
        player.Lives = lives;

        Debug.Log("Game initialized.");
    }

    public void StartGame()
    {
        Debug.Log("Game started.");
        currentWave = 1;
        Debug.Log($"Current wave: {currentWave}");
        waveManager.StartInfiniteWaves(); // Now starts infinite waves
    }

    public void RestartGame()
    {
        Debug.Log("Game restarted.");
        InitializeGame();
        StartGame();
    }

    public void EndGame()
    {
        Debug.Log("Game over.");
        leaderboard.AddScore(player);
    }

    public void LoseLife(int amount)
    {
        player.Lives -= amount;
        Debug.Log($"Player lost {amount} lives. Remaining: {player.Lives}");

        if (player.Lives <= 0)
        {
            EndGame();
        }
    }

    public void AddGold(int amount)
    {
        player.EarnGold(amount);
        Debug.Log($"Player earned {amount} gold. Total: {player.Gold}");
    }

    public bool SpendGold(int amount)
    {
        if (player.SpendGold(amount))
        {
            Debug.Log($"Player spent {amount} gold. Remaining: {player.Gold}");
            return true;
        }

        Debug.Log("Not enough gold!");
        return false;
    }
}
