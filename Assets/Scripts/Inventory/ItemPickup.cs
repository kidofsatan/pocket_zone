using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public InventoryItem item;
    public int quantity = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("��������� �������");
        if (other.CompareTag("Player"))
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                if (inventory.HasSpaceForItem(item, quantity))
                {
                    Debug.Log("��������� ��������");
                    inventory.AddItem(item, quantity);
                    AudioManager.Instance.PlaySFX(1);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("��������� ����������, �� ���� �������� �������.");
                }
            }
        }
    }
}
