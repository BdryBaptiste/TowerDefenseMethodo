using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour, IEnemy
{
    public delegate void EnemyDeathEvent();
    public event EnemyDeathEvent OnDeathEvent;

    [Header("Common Enemy Attributes")]
    public string Type;
    public int Health;
    public float Speed;
    public int Damage;
    public int Reward;

    protected NavMeshAgent agent;
    protected Transform target;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError($"{name} : Aucun NavMeshAgent trouvé sur cet ennemi !");
            return;
        }

        agent.speed = Speed;
    }


    protected virtual void Update()
    {
        if (agent != null && target != null)
            agent.SetDestination(target.position);

        CheckHealth();
    }

    public virtual void Initialize(Transform targetTransform)
    {
        if (targetTransform == null)
        {
            Debug.LogError($"{name}: Target transform est NULL dans Initialize !");
            return;
        }
        
        target = targetTransform;
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
        CheckHealth();
    }

    protected void CheckHealth()
    {
        if (Health <= 0)
            OnDeath();
    }

    protected virtual void OnDeath()
    {
        Debug.Log($"{name} est mort. Suppression de l'objet.");

        OnDeathEvent?.Invoke();
        GameManager.Instance.AddGold(Reward);
        Destroy(gameObject);
        Debug.Log($"{name} a bien été détruit !");
    }
}
