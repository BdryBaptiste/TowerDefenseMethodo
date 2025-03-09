using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void Initialize(Transform target);
    void TakeDamage(float damage);
}
