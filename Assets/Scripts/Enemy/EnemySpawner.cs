using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public Transform[] spawnPoints; // Массив точек спавна
    private List<int> usedSpawnPoints = new List<int>(); // Список занятых точек спавна
    public int MaxEnemyIndex = 1; // максимальных индекс типа врага
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Уничтожить текущий объект, если экземпляр уже существует
        }
    }

    public void SpawnEnemies()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn points are not set.");
            return;
        }

        usedSpawnPoints.Clear();

        for (int i = 0; i < 3; i++)
        {
            int spawnPointIndex;
            do
            {
                spawnPointIndex = Random.Range(0, spawnPoints.Length);
            } while (usedSpawnPoints.Contains(spawnPointIndex));
            usedSpawnPoints.Add(spawnPointIndex);

            Transform spawnPoint = spawnPoints[spawnPointIndex];

            if (spawnPoint == null)
            {
                Debug.LogError("Spawn point is null.");
                continue;
            }
            int enemyIndex = Random.Range(0, MaxEnemyIndex);
            // Достаем объект из пула
            GameObject enemy = ObjectPool.instance.GetObject(enemyIndex);
            if (enemy == null)
            {
                Debug.LogError("Failed to get enemy from pool.");
                continue;
            }

            enemy.transform.position = spawnPoint.position;
            enemy.transform.rotation = spawnPoint.rotation;
        }
    }
}
