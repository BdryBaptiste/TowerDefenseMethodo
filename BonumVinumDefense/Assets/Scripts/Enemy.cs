using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void EnemyDeathEvent();
    public event EnemyDeathEvent OnDeathEvent;
    
    [Header("Enemy Attributes")]
    public string Type;
    public int Health = 100; // Enemy's current health
    public float Speed = 3.5f; // Movement speed
    public int Damage = 1; // Damage dealt to the player on reaching the goal
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
        CheckHealth();
    }

    public void Move()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;

        if(damage <= Health)
        {
            Health -= Mathf.FloorToInt(damage);
            
        }
        else
        {
            Health = 0;
        }

        Debug.Log($"{name} took {damage} damage. Remaining health: {Health}");
    }

    private void CheckHealth()
    {
        if (Health <= 0)
        {
            Health = 0;
            OnDeath();
        }
    }

    public void ApplyEffect(EnemyEffect effect)
    {
        activeEffects.Add(new ActiveEffect(effect, Time.time));
        Debug.Log($"{name} affected by {effect.GetType().Name} for {effect.Duration} seconds.");
    }

    private void UpdateEffects()
    {
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

    private void ApplyEffectBehavior(EnemyEffect effect)
    {
        switch (effect)
        {
            case BurnEffect burn:
                if (Time.time >= burn.NextTickTime) // Check if it's time to apply damage
                {
                    TakeDamage(burn.DamagePerTick);
                    burn.NextTickTime = Time.time + burn.TickInterval; // Schedule next tick
                }
                break;

            case SlowEffect slow:
                Speed = Mathf.Max(baseSpeed * (1 - slow.SlowPercentage), 0.5f);
                break;

            case PoisonEffect poison:
                if (Time.time >= poison.NextTickTime)
                {
                    TakeDamage(poison.DamagePerTick);
                    poison.NextTickTime = Time.time + poison.TickInterval;
                }
                break;
        }
    }

    private void RemoveEffect(ActiveEffect activeEffect)
    {
        Debug.Log($"{name} no longer affected by {activeEffect.Effect.GetType().Name}.");
        activeEffects.Remove(activeEffect);

        if (activeEffect.Effect is SlowEffect)
        {
            Speed = baseSpeed;
        }
    }

    private void OnDeath()
    {
        OnDeathEvent?.Invoke();
        GameManager.Instance.AddGold(Reward);
        Debug.Log($"{name} died. Player earned {Reward} gold.");
        Destroy(gameObject);
    }
}