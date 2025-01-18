using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject gridPrefab; // Your 9x9 grid prefab
    private Node[,] gridNodes; // Array to store node references

    public int gridSize = 9; // Size of the grid (assume square)

    private void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        // Assuming the grid prefab is already placed in the scene
        gridNodes = new Node[gridSize, gridSize];

        // Loop through the prefab's child nodes and populate the gridNodes array
        Node[] nodes = gridPrefab.GetComponentsInChildren<Node>();

        int index = 0;
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                gridNodes[x, z] = nodes[index];
                index++;
            }
        }

        Debug.Log("Grid initialized with " + nodes.Length + " nodes.");
    }

    public Node GetNodeAtPosition(Vector3 worldPosition)
    {
        // Find the nearest node based on world position
        Node nearestNode = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Node node in gridNodes)
        {
            float distance = Vector3.Distance(node.transform.position, worldPosition);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestNode = node;
            }
        }

        return nearestNode;
    }

    public bool PlaceTower(GameObject turretPrefab, Vector3 position)
    {
        Node node = GetNodeAtPosition(position);
        if (node != null)
        {
            return node.PlaceTurret(turretPrefab);
        }

        Debug.LogWarning("No valid node found for turret placement!");
        return false;
    }

    public void RemoveTower(Vector3 position)
    {
        Node node = GetNodeAtPosition(position);
        if (node != null && node.isOccupied)
        {
            node.isOccupied = false; // Mark the node as free
            Debug.Log("Tower removed from node at " + position);
        }
    }
}
