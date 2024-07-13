using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    /// <summary>
    /// ячейка инвентаря
    /// </summary>
    public Image icon;
    public TMP_Text quantityText;
    public InventoryItem item;
    public Button removeItemButton;
    public int quantity;
    public Sprite emptySlotIcon; //пустая ячейка инвентаря

    // подписываемся на событие удаления объекта из инвентаря
    public delegate void OnRemoveItem(InventorySlot slot);
    public event OnRemoveItem onRemoveItem;

    // добавление предмета
    public void AddItem(InventoryItem newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
        icon.sprite = item.icon;
        icon.enabled = true;
        UpdateQuantity();
        Debug.Log("Initialized slot with item: " + item.name + ", quantity: " + quantity);
    }

    //очищение слота инвентаря
    public void ClearSlot()
    {
        item = null;
        quantity = 0;
        icon.sprite = emptySlotIcon;
        quantityText.text = "";
        HideRemoveButton();
    }

    
    // счетчик количества предмета
    public void UpdateQuantity()
    {
        if (quantity > 1)
        {
            quantityText.text = quantity.ToString();
        }
        else
        {
            quantityText.text = "";
        }
    }

   


    // кнопка удаления предмета из инвентаря
    public void OnRemoveButton()
    {
        Debug.Log("кнопка удаления нажата");
        if (onRemoveItem != null)
        {
            onRemoveItem(this);
        }
        HideRemoveButton();
    }

    //появление кнопки удаления предмета
    public void OnSlotClicked()
    {
        if (item != null)
        {
            removeItemButton.gameObject.SetActive(true);
        }
    }

    // сокрытие кнопки удаления предмета
    private void HideRemoveButton()
    {
        removeItemButton.gameObject.SetActive(false);
    }
}
