using UnityEngine;

public class Tower : MonoBehaviour
{
    public float Range { get; protected set; } = 5f; // Attack range
    public int Damage { get; protected set; } = 10; // Damage dealt per attack
    public float Cooldown { get; protected set; } = 1f; // Attacks per second

    public Transform firePoint; // Point from where projectiles are fired

    private float attackCooldown = 0f; // Time left until the next attack

    // Attack behavior strategy
    public ITowerStrategy attackStrategy;

    // Special effect applied by the tower
    public TowerEffect effect;

    private void Update()
    {
        // Check cooldown
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            Attack();
            attackCooldown = 1f / Cooldown;
        }
    }

    public void SetStrategy(ITowerStrategy strategy)
    {
        attackStrategy = strategy;
    }

    public virtual void Attack()
    {
        if (attackStrategy != null)
        {
            attackStrategy.Execute(this);
        }
        else
        {
            Debug.LogWarning("No attack strategy set!");
        }
    }

    public void UpgradeRange(float amount)
    {
        Range += amount;
        Debug.Log($"Range upgraded to {Range}");
    }

    public void UpgradeDamage(int amount)
    {
        Damage += amount;
        Debug.Log($"Damage upgraded to {Damage}");
    }

    public void UpgradeCooldown(float amount)
    {
        Cooldown = Mathf.Max(0.1f, Cooldown - amount); // Prevent cooldown from going below 0.1
        Debug.Log($"Cooldown upgraded to {Cooldown}");
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the range of the tower in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}