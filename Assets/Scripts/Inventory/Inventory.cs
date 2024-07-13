using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryMenu; // Префаб окна с меню инвентаря
    public GameObject slotPrefab; // Префаб ячейки
    public Transform itemsParent; // Родительский объект для ячеек
    public int numberOfSlots = 20; // Максимальное количество ячеек

    // Считаем боеприпасы
    public TextMeshProUGUI ammoText; // Используйте TextMeshProUGUI для UI текста
    public int ammoCount;

    private List<InventorySlot> slots = new List<InventorySlot>();

    public List<InventoryItem> allItems;

    void Start() 
    { 
        // Создаем ячейки инвентаря
        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, itemsParent);
            InventorySlot slotComponent = newSlot.GetComponent<InventorySlot>();
            slotComponent.onRemoveItem += RemoveItemFromSlot; // Подписываемся на событие InventorySlot
            slots.Add(slotComponent);
        }

        // Загружаем данные инвентаря
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

    // сохранение инвентаря при выходе
    void OnApplicationQuit()
    {
        SaveSystem.SaveInventory(this);
    }

    public InventoryItem GetItemByName(string itemName)
    {
        // Предполагается, что у вас есть список всех возможных предметов
        foreach (var item in allItems)
        {
            if (item.name == itemName)
            {
                return item;
            }
        }
        return null;
    }

    //проверка места для предмета

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

    // добавление предмета в инвентарь
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


    // метод использования патронов, начиная с последнего добавленного слота
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

    // счетчик патронов
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

    //удаление премдета из инвентаря
    public void RemoveItemFromSlot(InventorySlot slot)
    {
        Debug.Log("удаление предмета из слота");
        slot.ClearSlot();
        RearrangeInventory();
        UpdateAmmoCount();
    }

    // сортировка инвентаря
    private void RearrangeInventory()
    {
        List<InventoryItem> items = new List<InventoryItem>();
        List<int> quantities = new List<int>();

        // Собираем все предметы и их количество
        foreach (var slot in slots)
        {
            if (slot.item != null)
            {
                items.Add(slot.item);
                quantities.Add(slot.quantity);
                slot.ClearSlot();
            }
        }

        // Заполняем слоты собранными предметами
        for (int i = 0; i < items.Count; i++)
        {
            slots[i].AddItem(items[i], quantities[i]);
        }

        // Очищаем оставшиеся слоты
        for (int i = items.Count; i < slots.Count; i++)
        {
            slots[i].ClearSlot();
        }
        UpdateAmmoCount();
    }

    // обновление отображения патронов на UI
    private void UpdateAmmoUI()
    {
        ammoText.text = ammoCount.ToString();
    }

    // метод для кнопки открытия инвентаря
    public void OpenInventoryMenu()
    {
        inventoryMenu.SetActive(true);
        GameManager.instance.PauseGame();
    }

    // метод для кнопки закрытия инвентаря
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