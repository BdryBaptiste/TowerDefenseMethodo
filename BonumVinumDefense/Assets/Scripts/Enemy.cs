using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]
    public float health = 100f;
    public float speed = 3.5f;
    public int rewardOnDeath = 10; // Gold rewarded to player upon death

    private Transform goal; // Target goal point
    private NavMeshAgent agent;

    private void Start()
    {
        // Initialize NavMeshAgent and set its speed
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        // Set the goal to the endpoint of the level
        goal = GameObject.FindGameObjectWithTag("Goal").transform;
        agent.SetDestination(goal.position);
    }

    private void Update()
    {
        // Check if the enemy reached the goal
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            ReachGoal();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // Notify GameManager to reward the player
        GameManager.Instance.AddGold(rewardOnDeath);

        // Destroy this enemy
        Destroy(gameObject);
    }

    private void ReachGoal()
    {
        // Notify GameManager that a life should be lost
        GameManager.Instance.LoseLife(1);

        // Destroy this enemy
        Destroy(gameObject);
    }
}
