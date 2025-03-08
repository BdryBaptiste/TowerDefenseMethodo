using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage;
    private Transform target;

    public void SetTarget(Transform newTarget, int attackDamage)
    {
        target = newTarget;
        damage = attackDamage;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Destroy if target is lost
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject); // Destroy the projectile on impact
    }
}
