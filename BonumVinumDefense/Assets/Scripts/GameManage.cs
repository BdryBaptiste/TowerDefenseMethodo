using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<int> OnLivesChanged;
    public event Action<int> OnGoldChanged;

    public int currentWave = 0;

    public Map map;
    public Leaderboard leaderboard;
    public Player player;
    public WaveManager waveManager;
    public GameObject gameOverMenu;

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

        OnLivesChanged?.Invoke(player.Lives);
        OnGoldChanged?.Invoke(player.Gold);

        Debug.Log("Game initialized.");
    }

    public int GetLives()
    {
        return player.Lives;
    }

    public int GetGold()
    {
        return player.Gold;
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
        if(gameOverMenu == null)
        {
            Debug.LogError("Game over menu not set!");
            return;
        }
        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);
    }

    public void LoseLife(int amount)
    {
        player.Lives -= amount;
        Debug.Log($"Player lost {amount} lives. Remaining: {player.Lives}");

        OnLivesChanged?.Invoke(player.Lives);

        if (player.Lives <= 0)
        {
            EndGame();
        }
    }

    public void AddGold(int amount)
    {
        player.EarnGold(amount);
        OnGoldChanged?.Invoke(player.Gold);
        Debug.Log($"Player earned {amount} gold. Total: {player.Gold}");
    }

    public bool SpendGold(int amount)
    {
        if (player.SpendGold(amount))
        {
            OnGoldChanged?.Invoke(player.Gold);
            Debug.Log($"Player spent {amount} gold. Remaining: {player.Gold}");
            return true;
        }

        Debug.Log("Not enough gold!");
        return false;
    }
}
