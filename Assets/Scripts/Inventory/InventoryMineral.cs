using UnityEngine;

[CreateAssetMenu(fileName = "InventoryMineral", menuName = "Scriptable Objects/InventoryMineral")]
public class InventoryMineral : ScriptableObject
{
    public string mineralName;
    public Sprite icon;
    public int dropAmount;
    public int maxStack = 99;
    public int sellPrice;
    public MineralType mineralType;
    public MineralRarity rarity;
}

public enum MineralType
{
    Coal,
    Copper
}

public enum MineralRarity
{
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Mythic,
    Secret
}
