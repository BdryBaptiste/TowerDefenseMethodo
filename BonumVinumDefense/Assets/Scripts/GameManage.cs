using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int lives = 20; // Player lives
    public int currentWave = 0; // Current wave number
    public int gold = 100; // Player's starting gold

    public Map map; // Reference to the map
    public Leaderboard leaderboard; // Reference to the leaderboard
    public Player player; // Reference to the player
    public WaveManager waveManager; // Manages waves of enemies

    private void Awake()
    {
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
        // Initialize player and other game components
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
        waveManager.StartWave(currentWave);
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
        // Display Game Over UI and save scores
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

    public void CompleteWave()
    {
        currentWave++;
        Debug.Log($"Wave {currentWave - 1} completed. Starting wave {currentWave}.");

        player.UpdateScore(currentWave);
        waveManager.StartWave(currentWave);
    }
}
