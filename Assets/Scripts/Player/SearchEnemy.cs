using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchEnemy : MonoBehaviour
{
    EnemyBase enemy;

    [SerializeField]
    LayerMask layer;
    [SerializeField]
    float radius;

    Collider GetNearestCollider(float radius, LayerMask mask)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, mask);

        if (hitColliders == null)
        {
            return null;
        }

        Collider nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (var col in hitColliders)
        {
            Debug.Log(col.gameObject.name);
            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = col;
            }
        }
        return nearest;
    }

    public Vector3 GetEnemyPos()
    {
        enemy = GetNearestCollider(radius, layer)?.GetComponent<EnemyBase>();

        if (enemy == null)
        {
            return Vector3.zero;
        }

        return enemy.transform.position;
    }
}
