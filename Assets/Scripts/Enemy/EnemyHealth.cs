using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float currentHealth = 50;
    [SerializeField] private float maxHealth;
    [SerializeField] private Slider healthSlider;
    public Transform healthBarParent; // Родительский объект для слайдера
    private Camera mainCamera;
    [SerializeField] private List<GameObject> droppedItems;


    private void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Позиция родительского объекта слайдера здоровья
        Vector3 healthBarPosition = transform.position + new Vector3(0, 1, 0);
        healthBarParent.position = healthBarPosition;

        // Фиксируем вращение родительского объекта слайдера относительно камеры
        healthBarParent.rotation = Quaternion.LookRotation(healthBarParent.position - mainCamera.transform.position);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        if (currentHealth <= 0)
        {
            Debug.Log("Противник уничтожен");
            Die();
        }
    }

    void OnDisable()
    {
        // Возвращаем объект в пул при деактивации
        if (ObjectPool.instance != null)
        {
            ObjectPool.instance.ReturnObject(gameObject);
        }
    }

    //смерть противника и дроп премдета

    void Die()
    {
        
        if (droppedItems != null && droppedItems.Count > 0)
        {
            GameObject droppedItem = droppedItems[Random.Range(0, droppedItems.Count)];
            if (droppedItem != null)
            {
                Instantiate(droppedItem, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Выбранный предмет равен null.");
            }
        }
        else
        {
            Debug.LogError("Список droppedItems пуст или не инициализирован.");
        }
        //удаляем объект противника
        gameObject.SetActive(false);
    }
}



