using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    // Game state variables
    [SerializeField] private int lives = 20;
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int gold = 100;

    // References to other systems
    private Map map;
    private Leaderboard leaderboard;

    // Events for game state changes
    public delegate void GameStateChangeHandler();
    public static event GameStateChangeHandler OnGameStart;
    public static event GameStateChangeHandler OnGameEnd;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize other components
        map = FindObjectOfType<Map>();
        leaderboard = FindObjectOfType<Leaderboard>();
    }

    private void Start()
    {
        // Optional: Automatically start the game when the scene loads
        StartGame();
    }

    // Public methods

    public void StartGame()
    {
        Debug.Log("Game started");
        currentWave = 1;
        lives = 20;
        gold = 100;
        OnGameStart?.Invoke();
        
        // Start the first wave
        StartCoroutine(SpawnWave());
    }

    public void RestartGame()
    {
        Debug.Log("Game restarted");
        StartGame();
    }

    public void EndGame()
    {
        Debug.Log("Game ended");
        OnGameEnd?.Invoke();
        // Optionally handle leaderboard updates or display game over screen
    }

    public void LoseLife(int amount)
    {
        lives -= amount;
        if (lives <= 0)
        {
            EndGame();
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            return true;
        }
        return false;
    }

    private IEnumerator SpawnWave()
    {
        Debug.Log($"Spawning wave {currentWave}");

        // Replace with your WaveManager logic
        yield return new WaitForSeconds(2f);

        Debug.Log($"Wave {currentWave} completed");
        currentWave++;

        // Check for game completion or prepare next wave
    }
}
