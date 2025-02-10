using System.Collections.Generic;
using UnityEngine;

public class Leaderboard
{
    private List<Player> players = new List<Player>(); // List of players and their scores

    // Adds a player's score to the leaderboard
    public void AddScore(Player player)
    {
        players.Add(player);
        players.Sort((a, b) => b.Score.CompareTo(a.Score)); // Sort by score in descending order

        Debug.Log($"Added {player.Name} to the leaderboard with score: {player.Score}");
    }

    // Retrieves the top N scores
    public List<Player> GetTopScores(int topN)
    {
        int count = Mathf.Min(topN, players.Count);
        List<Player> topPlayers = players.GetRange(0, count);

        Debug.Log("Top Scores:");
        foreach (var player in topPlayers)
        {
            Debug.Log($"{player.Name}: {player.Score}");
        }

        return topPlayers;
    }

    // Displays the leaderboard in debug logs
    public void PrintLeaderboard()
    {
        Debug.Log("=== Leaderboard ===");
        foreach (var player in players)
        {
            Debug.Log($"{player.Name}: {player.Score}");
        }
    }
}