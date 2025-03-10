using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    private int globalDamageUpgrade = 0;
    private float globalRangeUpgrade = 0f;
    private float globalCooldownReduction = 0f;

    private int damageUpgradeCost = 50; // Cost of damage upgrade
    private int rangeUpgradeCost = 40; // Cost of range upgrade
    private int cooldownUpgradeCost = 60; // Cost of cooldown upgrade

    private List<Tower> towers = new List<Tower>(); // Track all towers

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public int GetGlobalDamageUpgrade() => globalDamageUpgrade;
    public float GetGlobalRangeUpgrade() => globalRangeUpgrade;
    public float GetGlobalCooldownReduction() => globalCooldownReduction;

    public void RegisterTower(Tower tower)
    {
        towers.Add(tower);
        tower.ApplyGlobalUpgrades(globalDamageUpgrade, globalRangeUpgrade, globalCooldownReduction);
    }

    public void UpgradeAllTowers(string upgradeType)
    {
        int upgradeCost = 0;

        switch (upgradeType)
        {
            case "Damage":
                upgradeCost = damageUpgradeCost;
                break;
            case "Range":
                upgradeCost = rangeUpgradeCost;
                break;
            case "Cooldown":
                upgradeCost = cooldownUpgradeCost;
                break;
            default:
                Debug.LogWarning("Invalid upgrade type.");
                return;
        }

        // Check if the player has enough gold
        if (!GameManager.Instance.SpendGold(upgradeCost))
        {
            Debug.LogWarning("Not enough gold to upgrade!");
            return;
        }

        // Apply the upgrade
        switch (upgradeType)
        {
            case "Damage":
                globalDamageUpgrade += 5;
                break;
            case "Range":
                globalRangeUpgrade += 2f;
                break;
            case "Cooldown":
                globalCooldownReduction += 0.2f;
                break;
        }

        // Update all towers with the new upgrade
        foreach (Tower tower in towers)
        {
            tower.ApplyGlobalUpgrades(globalDamageUpgrade, globalRangeUpgrade, globalCooldownReduction);
        }

        Debug.Log($"Applied global {upgradeType} upgrade! Cost: {upgradeCost} gold");
    }
}
