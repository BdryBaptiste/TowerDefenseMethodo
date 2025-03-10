using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string Name { get; private set; }
    public int Lives { get; set; } = 20; // Starting lives
    public int Score { get; private set; } = 0; // Player's score
    public int Gold { get; private set; } = 100; // Starting gold

    public Player(string name)
    {
        Name = name;
    }

    // Adds score based on waves survived or other achievements
    public void UpdateScore(int points)
    {
        Score += points;
        Debug.Log($"Score updated: {Score}");
    }

    // Deducts gold when spending
    public bool SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            Debug.Log($"Gold spent: {amount}. Remaining gold: {Gold}");
            return true;
        }

        Debug.Log("Not enough gold to spend.");
        return false;
    }

    // Adds gold to the player's total
    public void EarnGold(int amount)
    {
        Gold += amount;
        Debug.Log($"Gold earned: {amount}. Total gold: {Gold}");
    }

    // Handles life loss
    public void LoseLife(int amount)
    {
        Lives -= amount;
        Debug.Log($"Lives lost: {amount}. Remaining lives: {Lives}");

        if (Lives <= 0)
        {
            Debug.Log("Game Over.");
            // Call to GameManager or end game logic could go here
        }
    }
}
