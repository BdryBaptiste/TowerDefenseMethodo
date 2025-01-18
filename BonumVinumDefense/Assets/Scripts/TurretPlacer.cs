using UnityEngine;

public class TurretPlacer : MonoBehaviour
{
    public GameObject turretPrefab; // Turret to place

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Handle mouse input
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            AttemptPlaceTurret();
        }
    }

    private void AttemptPlaceTurret()
    {
        // Raycast to detect the node under the mouse
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Node node = hit.collider.GetComponent<Node>();
            if (node != null)
            {
                bool success = node.PlaceTurret(turretPrefab);
                if (success)
                {
                    Debug.Log("Turret placed!");
                }
            }
        }
    }
}