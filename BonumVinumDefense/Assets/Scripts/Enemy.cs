using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]
    public string Type;
    public int Health = 100; // Enemy's current health
    public float Speed = 3.5f; // Movement speed
    public int Damage = 10; // Damage dealt to the player on reaching the goal
    public int Reward = 10; // Gold rewarded on death

    // List of active effects applied to the enemy
    private List<ActiveEffect> activeEffects = new List<ActiveEffect>();

    private float baseSpeed; // Original speed for restoring after effects

    private void Start()
    {
        baseSpeed = Speed;
    }

    private void Update()
    {
        Move();
        UpdateEffects();
    }

    public void Move()
    {
        // Logic for enemy movement
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log($"{name} took {damage} damage. Remaining health: {Health}");

        if (Health <= 0)
        {
            OnDeath();
        }
    }

    public void ApplyEffect(TowerEffect effect)
    {
        // Add a new active effect
        ActiveEffect newEffect = new ActiveEffect(effect, Time.time);
        activeEffects.Add(newEffect);
        Debug.Log($"{name} is affected by {effect.EffectType} for {effect.Duration} seconds.");
    }

    private void UpdateEffects()
    {
        // Iterate through all active effects and update their behavior
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            ActiveEffect activeEffect = activeEffects[i];
            float elapsedTime = Time.time - activeEffect.StartTime;

            if (elapsedTime < activeEffect.Effect.Duration)
            {
                ApplyEffectBehavior(activeEffect.Effect);
            }
            else
            {
                RemoveEffect(activeEffect);
            }
        }
    }

    private void ApplyEffectBehavior(TowerEffect effect)
    {
        switch (effect.EffectType)
        {
            case "Burn":
                TakeDamage(Mathf.RoundToInt(effect.Magnitude * Time.deltaTime));
                break;

            case "Slow":
                Speed = Mathf.Max(baseSpeed * (1 - effect.Magnitude), 0.5f); // Slow down
                break;

            case "Poison":
                TakeDamage(Mathf.RoundToInt(effect.Magnitude * Time.deltaTime));
                break;
        }
    }

    private void RemoveEffect(ActiveEffect activeEffect)
    {
        Debug.Log($"{name} is no longer affected by {activeEffect.Effect.EffectType}.");
        activeEffects.Remove(activeEffect);

        // Reset speed if the effect was slowing
        if (activeEffect.Effect.EffectType == "Slow")
        {
            Speed = baseSpeed;
        }
    }

    private void OnDeath()
    {
        // Notify GameManager to reward the player
        GameManager.Instance.AddGold(Reward);
        Debug.Log($"{name} died. Rewarding player with {Reward} gold.");

        // Destroy the enemy
        Destroy(gameObject);
    }
}
