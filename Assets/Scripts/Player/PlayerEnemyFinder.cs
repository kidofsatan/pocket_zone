using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyFinder : MonoBehaviour
{
    public float searchRadius = 10f;

    // ����� ��� ������ ���������� ����� � �������
    public GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject enemy in enemies)
        {
            Vector3 direction = enemy.transform.position - position;
            float distance = direction.sqrMagnitude;
            if (distance < closestDistance && distance <= searchRadius * searchRadius)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private void OnDrawGizmosSelected()
    {
        // ������������� ���� Gizmos
        Gizmos.color = Color.red;

        // ������ ���������� � �������� ������ �����
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
