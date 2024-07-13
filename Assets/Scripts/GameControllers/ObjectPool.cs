using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public GameObject[] enemyPrefabs; // Массив префабов врагов
    public int poolSize = 10;

    private List<GameObject>[] pools;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        pools = new List<GameObject>[enemyPrefabs.Length];

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            pools[i] = new List<GameObject>();

            for (int j = 0; j < poolSize; j++)
            {
                GameObject obj = Instantiate(enemyPrefabs[i]);
                obj.SetActive(false);
                pools[i].Add(obj);
            }
        }
    }

    public GameObject GetObject(int enemyTypeIndex)
    {
        if (enemyTypeIndex >= 0 && enemyTypeIndex < pools.Length)
        {
            foreach (GameObject obj in pools[enemyTypeIndex])
            {
                if (obj != null && !obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }

            GameObject newObj = Instantiate(enemyPrefabs[enemyTypeIndex]);
            newObj.SetActive(true);
            pools[enemyTypeIndex].Add(newObj);
            return newObj;
        }
        else
        {
            Debug.LogWarning("Тип врага не найден в пуле!");
            return null;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
