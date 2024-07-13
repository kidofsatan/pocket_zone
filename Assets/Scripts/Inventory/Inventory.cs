using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryMenu; // ������ ���� � ���� ���������
    public GameObject slotPrefab; // ������ ������
    public Transform itemsParent; // ������������ ������ ��� �����
    public int numberOfSlots = 20; // ������������ ���������� �����

    // ������� ����������
    public TextMeshProUGUI ammoText; // ����������� TextMeshProUGUI ��� UI ������
    public int ammoCount;

    private List<InventorySlot> slots = new List<InventorySlot>();

    public List<InventoryItem> allItems;

    void Start() 
    { 
        // ������� ������ ���������
        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, itemsParent);
            InventorySlot slotComponent = newSlot.GetComponent<InventorySlot>();
            slotComponent.onRemoveItem += RemoveItemFromSlot; // ������������� �� ������� InventorySlot
            slots.Add(slotComponent);
        }

        // ��������� ������ ���������
        List<ItemData> loadedItems = SaveSystem.LoadInventory();
        Debug.Log("Loaded items count: " + loadedItems.Count);
        foreach (var itemData in loadedItems)
        {
            InventoryItem item = GetItemByName(itemData.itemName);
            if (item != null)
            {
                Debug.Log("Adding item: " + itemData.itemName + " Quantity: " + itemData.quantity);
                AddItem(item, itemData.quantity);
            }
            else
            {
                Debug.Log("Item not found: " + itemData.itemName);
            }
        }

        UpdateAmmoUI();
    }

    // ���������� ��������� ��� ������
    void OnApplicationQuit()
    {
        SaveSystem.SaveInventory(this);
    }

    public InventoryItem GetItemByName(string itemName)
    {
        // ��������������, ��� � ��� ���� ������ ���� ��������� ���������
        foreach (var item in allItems)
        {
            if (item.name == itemName)
            {
                return item;
            }
        }
        return null;
    }

    //�������� ����� ��� ��������

    public bool HasSpaceForItem(InventoryItem newItem, int quantity)
    {
        foreach (var slot in slots)
        {
            if (slot.item == newItem && slot.quantity < newItem.maxStack)
            {
                int totalQuantity = slot.quantity + quantity;
                if (totalQuantity <= newItem.maxStack)
                {
                    return true;
                }
            }
        }

        foreach (var slot in slots)
        {
            if (slot.item == null)
            {
                return true;
            }
        }

        return false;
    }

    // ���������� �������� � ���������
    public void AddItem(InventoryItem newItem, int quantity)
    {
        foreach (var slot in slots)
        {
            if (slot.item == newItem && slot.quantity < newItem.maxStack)
            {
                int totalQuantity = slot.quantity + quantity;
                if (totalQuantity <= newItem.maxStack)
                {
                    slot.AddItem(newItem, totalQuantity);
                    Debug.Log("Added item to existing slot: " + newItem.name + ", quantity: " + totalQuantity);
                    UpdateAmmoCount();
                    return;
                }
                else
                {
                    slot.AddItem(newItem, newItem.maxStack);
                    quantity = totalQuantity - newItem.maxStack;
                    Debug.Log("Added item to existing slot with overflow: " + newItem.name + ", quantity: " + newItem.maxStack);
                    UpdateAmmoCount();
                }
            }
        }

        foreach (var slot in slots)
        {
            if (slot.item == null)
            {
                slot.AddItem(newItem, quantity);
                Debug.Log("Added item to new slot: " + newItem.name + ", quantity: " + quantity);
                UpdateAmmoCount();
                return;
            }
        }
        UpdateAmmoCount();
    }


    // ����� ������������� ��������, ������� � ���������� ������������ �����
    public void UseAmmo(int amount)
    {
        for (int i = slots.Count - 1; i >= 0; i--)
        {
            if (slots[i].item != null && slots[i].item.name == "Ammo")
            {
                if (slots[i].quantity >= amount)
                {
                    slots[i].quantity -= amount;
                    UpdateAmmoCount();
                    return;
                }
                else
                {
                    amount -= slots[i].quantity;
                    slots[i].quantity = 0;
                    slots[i].ClearSlot();
                }
            }
        }
        UpdateAmmoCount();
    }

    // ������� ��������
    private void UpdateAmmoCount()
    {
        ammoCount = 0;
        foreach (var slot in slots)
        {
            if (slot.item != null && slot.item.name == "Ammo")
            {
                ammoCount += slot.quantity;
                slot.UpdateQuantity();
            }
        }
        UpdateAmmoUI();
    }

    //�������� �������� �� ���������
    public void RemoveItemFromSlot(InventorySlot slot)
    {
        Debug.Log("�������� �������� �� �����");
        slot.ClearSlot();
        RearrangeInventory();
        UpdateAmmoCount();
    }

    // ���������� ���������
    private void RearrangeInventory()
    {
        List<InventoryItem> items = new List<InventoryItem>();
        List<int> quantities = new List<int>();

        // �������� ��� �������� � �� ����������
        foreach (var slot in slots)
        {
            if (slot.item != null)
            {
                items.Add(slot.item);
                quantities.Add(slot.quantity);
                slot.ClearSlot();
            }
        }

        // ��������� ����� ���������� ����������
        for (int i = 0; i < items.Count; i++)
        {
            slots[i].AddItem(items[i], quantities[i]);
        }

        // ������� ���������� �����
        for (int i = items.Count; i < slots.Count; i++)
        {
            slots[i].ClearSlot();
        }
        UpdateAmmoCount();
    }

    // ���������� ����������� �������� �� UI
    private void UpdateAmmoUI()
    {
        ammoText.text = ammoCount.ToString();
    }

    // ����� ��� ������ �������� ���������
    public void OpenInventoryMenu()
    {
        inventoryMenu.SetActive(true);
        GameManager.instance.PauseGame();
    }

    // ����� ��� ������ �������� ���������
    public void CloseInventoryMenu()
    {
        inventoryMenu.SetActive(false);
        GameManager.instance.ResumeGame();
    }

    public List<InventorySlot> GetSlots()
    {
        return slots;
    }

    
}