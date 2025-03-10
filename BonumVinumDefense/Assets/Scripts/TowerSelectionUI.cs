using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionUI : MonoBehaviour
{
    public TurretPlacer selectionManager;
    public UpgradeManager upgradeManager;

    public Button singleTargetButton;
    public Button aoeButton;

    public Button burnButton;
    public Button slowButton;
    public Button poisonButton;

    public Button upgradeDamageButton;
    public Button upgradeRangeButton;
    public Button upgradeCooldownButton;

    private Color defaultColor = Color.white;
    private Color selectedColor = Color.green;

    private void Start()
    {
        // Attach button click listeners
        singleTargetButton.onClick.AddListener(() => SelectStrategy("SingleTarget", singleTargetButton));
        aoeButton.onClick.AddListener(() => SelectStrategy("AOE", aoeButton));

        burnButton.onClick.AddListener(() => SelectEffect("Burn", burnButton));
        slowButton.onClick.AddListener(() => SelectEffect("Slow", slowButton));
        poisonButton.onClick.AddListener(() => SelectEffect("Poison", poisonButton));

        upgradeDamageButton.onClick.AddListener(() => UpgradeAllTowers("Damage"));
        upgradeRangeButton.onClick.AddListener(() => UpgradeAllTowers("Range"));
        upgradeCooldownButton.onClick.AddListener(() => UpgradeAllTowers("Cooldown"));
    }

    private void SelectStrategy(string strategyType, Button selectedButton)
    {
        selectionManager.SetSelectedStrategy(strategyType);
        UpdateStrategyUI(selectedButton);
    }

    private void SelectEffect(string effectType, Button selectedButton)
    {
        selectionManager.SetSelectedEffect(effectType);
        UpdateEffectUI(selectedButton);
    }

    private void UpdateStrategyUI(Button selectedButton)
    {
        // Reset all buttons
        singleTargetButton.image.color = defaultColor;
        aoeButton.image.color = defaultColor;

        // Highlight selected
        selectedButton.image.color = selectedColor;
    }

    private void UpdateEffectUI(Button selectedButton)
    {
        // Reset all buttons
        burnButton.image.color = defaultColor;
        slowButton.image.color = defaultColor;
        poisonButton.image.color = defaultColor;

        // Highlight selected
        selectedButton.image.color = selectedColor;
    }

    private void UpgradeAllTowers(string upgradeType)
    {
        upgradeManager.UpgradeAllTowers(upgradeType);
        Debug.Log($"Global {upgradeType} upgrade applied to all towers!");
    }
}