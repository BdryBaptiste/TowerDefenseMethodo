using UnityEngine;

public class Tower : MonoBehaviour
{
    public float Range { get; protected set; } = 5f; // Attack range
    public int Damage { get; protected set; } = 10; // Damage dealt per attack
    public float Cooldown { get; protected set; } = 1f; // Attacks per second

    public Transform firePoint; // Point from where projectiles are fired
    public Transform visualHolder;

    public GameObject burnVisualPrefab;
    public GameObject slowVisualPrefab;
    public GameObject poisonVisualPrefab;

    private float attackCooldown = 0f; // Time left until the next attack

    // Attack behavior strategy
    public ITowerStrategy attackStrategy;

    private TowerDecorator effectDecorator;

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
        Debug.Log($"Tower : Tower set to {strategy.GetType().Name} mode.");
    }

    public void SetEffect(string effectType)
    {
        switch (effectType)
        {
            case "Burn":
                effectDecorator = new BurnEffectDecorator(attackStrategy, 5f, 0.5f, 3f);
                UpdateTowerVisual(burnVisualPrefab);
                break;
            case "Slow":
                effectDecorator = new SlowEffectDecorator(attackStrategy, 0.5f, 3f);
                UpdateTowerVisual(slowVisualPrefab);
                break;
            case "Poison":
                effectDecorator = new PoisonEffectDecorator(attackStrategy, 6f, 1f, 10f);
                UpdateTowerVisual(poisonVisualPrefab);
                break;
        }
    }

    public void Attack()
    {
        if (effectDecorator != null)
        {
            effectDecorator.Execute(this); // ✅ Decorator handles effects
        }
        else if (attackStrategy != null)
        {
            attackStrategy.Execute(this);
        }
    }


    private void UpdateTowerVisual(GameObject visualPrefab)
    {
        Debug.Log("Tower : Updating visual effect");
        if (visualHolder == null) return;

        foreach (Transform child in visualHolder)
        {
            Debug.Log($"Tower : Destroying {child.gameObject.name}");
            Destroy(child.gameObject);
        }

        if (visualPrefab != null)
        {
            Debug.Log($"Tower : Instantiating {visualPrefab.name}");
            Instantiate(visualPrefab, visualHolder.position, visualHolder.rotation, visualHolder);
        }
    }

    public void UpgradeRange(float amount)
    {
        Range += amount;
        Debug.Log($"Tower : Range upgraded to {Range}");
    }

    public void UpgradeDamage(int amount)
    {
        Damage += amount;
        Debug.Log($"Tower : Damage upgraded to {Damage}");
    }

    public void UpgradeCooldown(float amount)
    {
        Cooldown = Mathf.Max(0.1f, Cooldown - amount); // Prevent cooldown from going below 0.1
        Debug.Log($"Tower : Cooldown upgraded to {Cooldown}");
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the range of the tower in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}