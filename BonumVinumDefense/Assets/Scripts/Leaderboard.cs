using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    // Data structure to store player scores
    private List<PlayerScore> playerScores = new List<PlayerScore>();

    public void AddScore(string playerName, int score)
    {
        // Check if player already exists
        PlayerScore existingPlayer = playerScores.Find(player => player.Name == playerName);

        if (existingPlayer != null)
        {
            // Update the score if it's higher
            if (score > existingPlayer.Score)
            {
                existingPlayer.Score = score;
            }
        }
        else
        {
            // Add a new player score
            playerScores.Add(new PlayerScore(playerName, score));
        }

        // Sort the leaderboard by score in descending order
        playerScores.Sort((a, b) => b.Score.CompareTo(a.Score));
    }

    public List<PlayerScore> GetTopScores(int count)
    {
        // Return the top scores, limited by the requested count
        return playerScores.GetRange(0, Mathf.Min(count, playerScores.Count));
    }

    public void PrintLeaderboard()
    {
        Debug.Log("=== Leaderboard ===");
        foreach (PlayerScore score in playerScores)
        {
            Debug.Log(score.Name + ": " + score.Score);
        }
    }
}
