using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New inventory Item", menuName = "Item Data")]
public class InventoryItem :ScriptableObject 
{
    public string itemName;
    public Sprite icon;
    public int maxStack;
}
