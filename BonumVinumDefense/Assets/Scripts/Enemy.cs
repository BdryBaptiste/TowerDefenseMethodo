using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public event Action OnDeathEvent;
    public event Action<Enemy> OnReachGoal;
    
    [Header("Enemy Attributes")]
    public string Type;
    public int Health = 100; // Enemy's current health
    public float Speed = 3.5f; // Movement speed
    public int Damage = 1; // Damage dealt to the player on reaching the goal
    public int Reward = 10; // Gold rewarded on death

    // List of active effects applied to the enemy
    private List<ActiveEffect> activeEffects = new List<ActiveEffect>();

    private float baseSpeed; // Original speed for restoring after effects

    [Header("NavMesh Settings")]
    public NavMeshAgent agent;
    public Transform target;

    private void Start()
    {
        baseSpeed = Speed;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;

        if (target != null)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            Debug.LogWarning("La cible (target) n'a pas été assignée à l'ennemi !");
        }
    }

    private void Update()
    {
        Move();
        UpdateEffects();
        CheckHealth();
        CheckGoal();
    }

    public void Move()
    {
        if(agent != null && target != null)
        {
            agent.SetDestination(target.position);
        }
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

    private void CheckGoal()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            ReachGoal();
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

    private void ReachGoal()
    {
        Debug.Log($"{gameObject.name} reached the goal! Dealing {Damage} damage.");
        OnReachGoal?.Invoke(this); // Calls event to reduce life points
        Destroy(gameObject);
    }
}