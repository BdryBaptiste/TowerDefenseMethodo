using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETargetStrategy : ITowerStrategy
{
    public void Execute(Tower tower)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(tower.transform.position, tower.Range);
        foreach (Collider hit in hitEnemies)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(tower.Damage);
            }
        }
        Debug.Log("AOE Target: Damaged multiple enemies.");
    }
}