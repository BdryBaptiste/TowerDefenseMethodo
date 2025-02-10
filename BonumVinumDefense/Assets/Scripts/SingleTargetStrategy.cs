using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetStrategy : ITowerStrategy
{
    public void Execute(Tower tower)
    {
        Enemy target = FindNearestEnemy(tower.transform.position, tower.Range);
        if (target != null)
        {
            target.TakeDamage(tower.Damage);
            Debug.Log("Single Target: Damaged " + target.name);
        }
    }

    private Enemy FindNearestEnemy(Vector3 position, float range)
    {
        // Logic to find the nearest enemy within range
        return null; // Placeholder
    }
}