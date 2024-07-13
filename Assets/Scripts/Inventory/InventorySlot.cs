using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    /// <summary>
    /// ������ ���������
    /// </summary>
    public Image icon;
    public TMP_Text quantityText;
    public InventoryItem item;
    public Button removeItemButton;
    public int quantity;
    public Sprite emptySlotIcon; //������ ������ ���������

    // ������������� �� ������� �������� ������� �� ���������
    public delegate void OnRemoveItem(InventorySlot slot);
    public event OnRemoveItem onRemoveItem;

    // ���������� ��������
    public void AddItem(InventoryItem newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
        icon.sprite = item.icon;
        icon.enabled = true;
        UpdateQuantity();
        Debug.Log("Initialized slot with item: " + item.name + ", quantity: " + quantity);
    }

    //�������� ����� ���������
    public void ClearSlot()
    {
        item = null;
        quantity = 0;
        icon.sprite = emptySlotIcon;
        quantityText.text = "";
        HideRemoveButton();
    }

    
    // ������� ���������� ��������
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

   


    // ������ �������� �������� �� ���������
    public void OnRemoveButton()
    {
        Debug.Log("������ �������� ������");
        if (onRemoveItem != null)
        {
            onRemoveItem(this);
        }
        HideRemoveButton();
    }

    //��������� ������ �������� ��������
    public void OnSlotClicked()
    {
        if (item != null)
        {
            removeItemButton.gameObject.SetActive(true);
        }
    }

    // �������� ������ �������� ��������
    private void HideRemoveButton()
    {
        removeItemButton.gameObject.SetActive(false);
    }
}
