using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavesData
{
    public List<ItemData> items = new List<ItemData>();
    public float playerHealth;
}

[System.Serializable]
public class ItemData
{
    public string itemName;
    public int quantity;
}

[System.Serializable]
public class PlayerData
{
    public float health;
    public float positionX;
    public float positionY;
    public float positionZ;

    public PlayerData(PlayerStats player)
    {
        health = player.currentHealth;
        positionX = player.transform.position.x;
        positionY = player.transform.position.y;
        positionZ = player.transform.position.z;
    }
}