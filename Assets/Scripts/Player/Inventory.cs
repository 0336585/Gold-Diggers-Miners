using UnityEngine;
using System.Collections.Generic;

// Define an enum for different mineral types
public enum MineralType
{
    Coal,
    Copper,
    Iron,
    Nickel,
    Silver,
    Tin,
    Cobalt,
    Gold,
    Diamond,
    Ruby,
    Emerald,
    Adamantium,
    Mythril,
    Orichalcum,
    Hellstone
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    // Dictionary to store minerals and their quantities
    private Dictionary<MineralType, int> minerals = new Dictionary<MineralType, int>();

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate inventory instances
            return;
        }

        Instance = this;
    }

    // Method to add minerals to the inventory
    public void AddMineral(MineralType mineral, int amount)
    {
        if (minerals.ContainsKey(mineral))
        {
            minerals[mineral] += amount;
        }
        else
        {
            minerals.Add(mineral, amount);
        }

        Debug.Log(mineral + " added. Current amount: " + minerals[mineral]);
    }

    // Method to remove minerals from the inventory
    public void RemoveMineral(MineralType mineral, int amount)
    {
        if (minerals.ContainsKey(mineral) && minerals[mineral] >= amount)
        {
            minerals[mineral] -= amount;
            Debug.Log(mineral + " removed. Current amount: " + minerals[mineral]);
        }
        else
        {
            Debug.Log("Not enough " + mineral + " to remove.");
        }
    }

    // Method to check the amount of a mineral in the inventory
    public int GetMineralAmount(MineralType mineral)
    {
        if (minerals.ContainsKey(mineral))
        {
            return minerals[mineral];
        }
        else
        {
            return 0;
        }
    }
}
