using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnDeathEvent;
    public event Action<Enemy> OnReachGoal;
    
    [Header("Enemy Attributes")]
    public string Type;
    public int Health = 50; // Enemy's current health
    public float Speed = 3.5f; // Movement speed
    public int Damage = 1; // Damage dealt to the player on reaching the goal
    public int Reward = 10; // Gold rewarded on death

    // List of active effects applied to the enemy
    private List<ActiveEffect> activeEffects = new List<ActiveEffect>();

    private float baseSpeed; // Original speed for restoring after effects
    private float baseHealth = 50;

    [Header("NavMesh Settings")]
    public NavMeshAgent agent;
    public Transform target;

    public GameObject burnEffect;
    public GameObject slowEffect;
    public GameObject poisonEffect;

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

    public void ScaleStats(int waveNumber)
    {
        Health += Mathf.CeilToInt(baseHealth + (waveNumber * 25)); // Increase health every wave
        baseSpeed += baseSpeed * (waveNumber * 0.2f); // Increase speed every wave

        Debug.Log($"Scaling Enemy - Health: {Health}, Speed: {baseSpeed} Wave: {waveNumber}");
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
        if (agent == null || !agent.enabled || !agent.isOnNavMesh) return;

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
            if (burnEffect)
            {
                burnEffect.SetActive(true);
                var burnParticles = burnEffect.GetComponent<ParticleSystem>();
                if (burnParticles && !burnParticles.isPlaying)
                    burnParticles.Play(); // Force play
            }
            if (Time.time >= burn.NextTickTime)
            {
                TakeDamage(burn.DamagePerTick);
                burn.NextTickTime = Time.time + burn.TickInterval;
            }
            break;

        case SlowEffect slow:
            if (slowEffect)
            {
                slowEffect.SetActive(true);
                var slowParticles = slowEffect.GetComponent<ParticleSystem>();
                if (slowParticles && !slowParticles.isPlaying)
                    slowParticles.Play(); // Force play
            }
            Speed = Mathf.Max(baseSpeed * (1 - slow.SlowPercentage), 0.5f);
            break;

        case PoisonEffect poison:
            if (poisonEffect)
            {
                poisonEffect.SetActive(true);
                var poisonParticles = poisonEffect.GetComponent<ParticleSystem>();
                if (poisonParticles && !poisonParticles.isPlaying)
                    poisonParticles.Play(); // Force play
            }
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
        if (slowEffect)
        {
            var slowParticles = slowEffect.GetComponent<ParticleSystem>();
            if (slowParticles) slowParticles.Stop(); // Stop slow effect
            slowEffect.SetActive(false);
        }
    }
    else if (activeEffect.Effect is BurnEffect)
    {
        if (burnEffect)
        {
            var burnParticles = burnEffect.GetComponent<ParticleSystem>();
            if (burnParticles) burnParticles.Stop(); // Stop burn effect
            burnEffect.SetActive(false);
        }
    }
    else if (activeEffect.Effect is PoisonEffect)
    {
        if (poisonEffect)
        {
            var poisonParticles = poisonEffect.GetComponent<ParticleSystem>();
            if (poisonParticles) poisonParticles.Stop(); // Stop poison effect
            poisonEffect.SetActive(false);
        }
    }
    }

    private void OnDeath()
    {
        OnDeathEvent?.Invoke(this);
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