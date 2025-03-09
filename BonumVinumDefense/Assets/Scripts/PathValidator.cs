using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PathValidator : MonoBehaviour
{
    public Transform enemySpawnPoint;
    public Transform enemyGoal;
    private NavMeshAgent dummyAgent;

    private void Start()
    {
        GameObject dummy = new GameObject("DummyNavAgent");
        dummyAgent = dummy.AddComponent<NavMeshAgent>();
        dummyAgent.speed = 0f;
        dummyAgent.radius = 0.5f;
        dummyAgent.enabled = false;
    }

    public IEnumerator IsPlacementValidCoroutine(System.Action<bool> callback)
    {
        if (enemySpawnPoint == null || enemyGoal == null)
        {
            Debug.LogWarning("PathValidator: Missing spawn or goal reference.");
            callback(false);
            yield break;
        }

        yield return new WaitForSeconds(0.1f); // Small delay for NavMesh update

        dummyAgent.enabled = true;
        NavMeshPath path = new NavMeshPath();
        bool hasPath = dummyAgent.CalculatePath(enemyGoal.position, path);
        dummyAgent.enabled = false;

        bool isValid = hasPath && path.status == NavMeshPathStatus.PathComplete;
        callback(isValid);
    }
}