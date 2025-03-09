using UnityEngine;

public class TowerFactory
{
    private GameObject towerPrefab;

    public TowerFactory(GameObject prefab)
    {
        towerPrefab = prefab; // Assign prefab in constructor
    }

    public Tower CreateTower(Vector3 position, Quaternion rotation, ITowerStrategy strategy, string effectType)
    {
        GameObject towerObj = Object.Instantiate(towerPrefab, position, rotation);
        Tower tower = towerObj.GetComponent<Tower>();

        if (tower != null)
        {
            tower.SetStrategy(strategy);
            tower.SetEffect(effectType);
        }

        return tower;
    }
}
