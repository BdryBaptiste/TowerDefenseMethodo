using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Attributes")]
    public float range = 5f; // Attack range
    public float damage = 10f; // Damage dealt per attack
    public float attackSpeed = 1f; // Attacks per second

    [Header("Tower Components")]
    public Transform target; // Current enemy target
    public Transform firePoint; // Point from where projectiles are fired

    private float attackCooldown = 0f; // Time left until the next attack

    private void Update()
    {
        // Check for a target
        if (target == null || !IsTargetInRange())
        {
            FindTarget();
            return;
        }

        // Attack cooldown management
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            Attack();
            attackCooldown = 1f / attackSpeed;
        }
    }

    private void FindTarget()
    {
        // Find the nearest enemy within range
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float shortestDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private bool IsTargetInRange()
    {
        return Vector3.Distance(transform.position, target.position) <= range;
    }

    private void Attack()
    {
        // Example attack logic (e.g., instantiate a projectile)
        Debug.Log("Attacking " + target.name);

        // If using projectiles, implement shooting logic here
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the range of the tower in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
