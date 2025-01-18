using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isOccupied = false; // Tracks if a turret is placed on this node

    private Renderer renderer;
    private Color originalColor;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;

        // Make the node invisible at the start
        SetTransparency(0f);
    }

    private void OnMouseEnter()
    {
        if (isOccupied)
        {
            SetColor(Color.red, 0.5f); // Semi-transparent red if occupied
        }
        else
        {
            SetColor(originalColor, 0.5f); // Semi-transparent original color
        }
    }

    private void OnMouseExit()
    {
        // Make the node invisible again
        SetTransparency(0f);
    }

    public bool PlaceTurret(GameObject turretPrefab)
    {
        if (!isOccupied)
        {
            Instantiate(turretPrefab, transform.position, Quaternion.identity);
            isOccupied = true;
            return true; // Placement successful
        }
        else
        {
            Debug.Log("Node is already occupied!");
            return false; // Placement failed
        }
    }

    private void SetColor(Color color, float alpha)
    {
        // Update the material's color to adjust transparency and color
        color.a = alpha;
        renderer.material.color = color;

        // Enable the renderer when alpha > 0, otherwise disable it
        renderer.enabled = alpha > 0f;
    }

    private void SetTransparency(float alpha)
    {
        // Retain the original color but adjust transparency
        SetColor(originalColor, alpha);
    }
}
