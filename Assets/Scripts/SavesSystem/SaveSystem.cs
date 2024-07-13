using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/inventory.json";
    private static string playerSavePath = Application.persistentDataPath + "/player.json";

    public static void SaveInventory(Inventory inventory)
    {
        Debug.Log("��������� ���������");
        SavesData data = new SavesData();
        foreach (var slot in inventory.GetSlots())
        {
            if (slot.item != null)
            {
                ItemData itemData = new ItemData
                {
                    itemName = slot.item.name,
                    quantity = slot.quantity
                };
                data.items.Add(itemData);
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("��������� ��������: " + json);
    }

    public static List<ItemData> LoadInventory()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SavesData data = JsonUtility.FromJson<SavesData>(json);
            Debug.Log("��������� ��������: " + json);
            return data.items;
        }
        else
        {
            Debug.LogWarning("��� ������ ���������: " + savePath);
            return new List<ItemData>();
        }
    }

    public static void SavePlayer(PlayerStats player)
    {
        Debug.Log("��������� ������ ������");
        PlayerData data = new PlayerData(player);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(playerSavePath, json);
        Debug.Log("������ ������ ���������: " + json);
    }

    public static PlayerData LoadPlayer()
    {
        if (File.Exists(playerSavePath))
        {
            string json = File.ReadAllText(playerSavePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("������ ������ ���������: " + json);
            return data;
        }
        else
        {
            Debug.LogWarning("��� ������ ������: " + playerSavePath);
            return null;
        }
    }

    // ����� ���� ������
    public static void ResetAllData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("������ ��������� ��������.");
        }

        if (File.Exists(playerSavePath))
        {
            File.Delete(playerSavePath);
            Debug.Log("������ ������ ��������.");
        }
    }
}