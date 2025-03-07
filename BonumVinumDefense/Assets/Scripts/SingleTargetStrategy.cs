using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetStrategy : ITowerStrategy
{
    public void Execute(Tower tower)
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        Enemy nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(tower.transform.position, enemy.transform.position);
            if (distance < shortestDistance && distance <= tower.Range)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            nearestEnemy.TakeDamage(tower.Damage);
            Debug.Log($"{tower.name} attacked {nearestEnemy.name} with {tower.Damage} damage.");
        }
    }
}