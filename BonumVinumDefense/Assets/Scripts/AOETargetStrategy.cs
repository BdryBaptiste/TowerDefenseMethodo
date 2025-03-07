using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETargetStrategy : ITowerStrategy
{
    public void Execute(Tower tower)
    {
        Collider[] hitColliders = Physics.OverlapSphere(tower.transform.position, tower.Range);
        foreach (Collider hit in hitColliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(tower.Damage);
                Debug.Log($"{tower.name} hit {enemy.name} with AOE damage {tower.Damage}.");
            }
        }
    }
}