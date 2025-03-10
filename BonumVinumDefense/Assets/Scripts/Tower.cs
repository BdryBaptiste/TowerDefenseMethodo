using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public virtual float Range { get; protected set; } = 5f; // Attack range
    public virtual int Damage { get; protected set; } = 10; // Damage dealt per attack
    public virtual float Cooldown { get; protected set; } = 1f; // Attacks per second

    public Transform firePoint; // Point from where projectiles are fired
    public Transform visualHolder;

    public GameObject burnVisualPrefab;
    public GameObject slowVisualPrefab;
    public GameObject poisonVisualPrefab;

    private float attackCooldown = 0f; // Time left until the next attack

    // Attack behavior strategy
    public ITowerStrategy attackStrategy;

    private TowerDecorator effectDecorator;
    private List<TowerUpgradeDecorator> upgrades = new List<TowerUpgradeDecorator>();

    private void Start()
    {
        UpgradeManager.Instance.RegisterTower(this);
        ApplyGlobalUpgrades(UpgradeManager.Instance.GetGlobalDamageUpgrade(), 
                            UpgradeManager.Instance.GetGlobalRangeUpgrade(), 
                            UpgradeManager.Instance.GetGlobalCooldownReduction());
    }

    private void Update()
    {
        // Check cooldown
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            Attack();
            attackCooldown = Cooldown;
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

    public virtual void Attack()
    {
        if (effectDecorator != null)
        {
            effectDecorator.Execute(this); // Decorator handles effects
        }
        else if (attackStrategy != null)
        {
            attackStrategy.Execute(this);
        }
    }

    public void ApplyGlobalUpgrades(int damageBonus, float rangeBonus, float cooldownReduction)
    {
        Damage = Damage + damageBonus;
        Range = Range + rangeBonus;
        Cooldown = Mathf.Max(0.1f, Cooldown - cooldownReduction);

        Debug.Log($" Tower updated! Damage: {Damage}, Range: {Range}, Cooldown: {Cooldown}");
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

    public void ApplyUpgrade(TowerUpgradeDecorator upgrade)
    {
        upgrades.Add(upgrade);
        Debug.Log($"Applied upgrade: {upgrade.GetType().Name}");
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the range of the tower in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}