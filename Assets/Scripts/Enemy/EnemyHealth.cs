using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float currentHealth = 50;
    [SerializeField] private float maxHealth;
    [SerializeField] private Slider healthSlider;
    public Transform healthBarParent; // ������������ ������ ��� ��������
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
        // ������� ������������� ������� �������� ��������
        Vector3 healthBarPosition = transform.position + new Vector3(0, 1, 0);
        healthBarParent.position = healthBarPosition;

        // ��������� �������� ������������� ������� �������� ������������ ������
        healthBarParent.rotation = Quaternion.LookRotation(healthBarParent.position - mainCamera.transform.position);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        if (currentHealth <= 0)
        {
            Debug.Log("��������� ���������");
            Die();
        }
    }

    void OnDisable()
    {
        // ���������� ������ � ��� ��� �����������
        if (ObjectPool.instance != null)
        {
            ObjectPool.instance.ReturnObject(gameObject);
        }
    }

    //������ ���������� � ���� ��������

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
                Debug.LogError("��������� ������� ����� null.");
            }
        }
        else
        {
            Debug.LogError("������ droppedItems ���� ��� �� ���������������.");
        }
        //������� ������ ����������
        gameObject.SetActive(false);
    }
}



