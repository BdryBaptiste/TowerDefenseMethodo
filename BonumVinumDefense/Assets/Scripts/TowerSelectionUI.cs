using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionUI : MonoBehaviour
{
    public TurretPlacer selectionManager;

    public Button singleTargetButton;
    public Button aoeButton;

    public Button burnButton;
    public Button slowButton;
    public Button poisonButton;

    private void Start()
    {
        // Attach button click listeners
        singleTargetButton.onClick.AddListener(() => selectionManager.SetSelectedStrategy("SingleTarget"));
        aoeButton.onClick.AddListener(() => selectionManager.SetSelectedStrategy("AOE"));

        burnButton.onClick.AddListener(() => selectionManager.SetSelectedEffect("Burn"));
        slowButton.onClick.AddListener(() => selectionManager.SetSelectedEffect("Slow"));
        poisonButton.onClick.AddListener(() => selectionManager.SetSelectedEffect("Poison"));
    }
}