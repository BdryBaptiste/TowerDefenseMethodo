using UnityEngine;

public class TurretPlacer : MonoBehaviour
{
    public GameObject turretPrefab; // Turret to place
    public LayerMask nodeLayerMask; // Layer mask for nodes

    private bool isPlacingMode = true;
    public string selectedStrategy = "SingleTarget"; // Default strategy
    public string selectedEffect = "Burn"; // Default effect

    private Camera mainCamera;
    private TowerFactory towerFactory;

    private void Start()
    {
        mainCamera = Camera.main;
        towerFactory = new TowerFactory(turretPrefab);
    }

    private void Update()
    {
        if (!PauseMenu.isPaused)
        {
            //Handle keyboard input
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                TogglePlacementMode();
            }

            if (Input.GetMouseButtonDown(0)) // Left Click
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (isPlacingMode)
                    {
                        AttemptPlaceTurret(hit.point);
                    }
                }
            }
        }
    }

    public void TogglePlacementMode()
    {
        isPlacingMode = !isPlacingMode;
        Debug.Log(isPlacingMode ? "Placement Mode Enabled" : "Selection Mode Enabled");
    }

    private void AttemptPlaceTurret(Vector3 position)
    {
        Ray ray = new Ray(position + Vector3.up * 10, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, nodeLayerMask))
        {
            Node node = hit.collider.GetComponent<Node>();
            if (node != null && !node.isOccupied)
            {
                // Apply selected strategy
                ITowerStrategy strategy = selectedStrategy == "SingleTarget" ? new SingleTargetStrategy() : new AOETargetStrategy();

                Tower newTower = towerFactory.CreateTower(node.transform.position, Quaternion.identity, strategy, selectedEffect);
                if (newTower != null)
                {
                    Debug.Log($"Tower placed at {position} with {strategy.GetType().Name} and effect {selectedEffect}");
                }

                node.isOccupied = true;
            }
        }
    }

    public void SetSelectedStrategy(string strategyType)
    {
        selectedStrategy = strategyType;
        Debug.Log($"Selected Strategy: {selectedStrategy}");
    }

    public void SetSelectedEffect(string effectType)
    {
        selectedEffect = effectType;
        Debug.Log($"Selected Effect: {selectedEffect}");
    }
}