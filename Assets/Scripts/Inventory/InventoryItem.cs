using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/InventoryItem")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject prefab;
    public ItemType itemType;
}

public enum ItemType
{
    RangedWeapon,
    MeleeWeapon,
    Throwable,
    Tool
}
