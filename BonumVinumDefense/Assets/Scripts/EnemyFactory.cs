using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Standard,
    Fast
}

public class EnemyFactory : MonoBehaviour
{
    public GameObject standardEnemyPrefab;
    public GameObject fastEnemyPrefab;

    public Transform enemyTarget;

    public GameObject CreateEnemy(EnemyType type, Vector3 spawnPosition)
    {
        GameObject enemyObject = null;

        switch (type)
        {
            case EnemyType.Standard:
                enemyObject = Instantiate(standardEnemyPrefab, spawnPosition, Quaternion.identity);
                break;

            case EnemyType.Fast:
                enemyObject = Instantiate(fastEnemyPrefab, spawnPosition, Quaternion.identity);
                break;

            default:
                Debug.LogError("Type d'ennemi non reconnu");
                return null;
        }

        var enemyComponent = enemyObject.GetComponent<IEnemy>();
        enemyComponent.Initialize(enemyTarget);
        
        return enemyObject;
    }
}
