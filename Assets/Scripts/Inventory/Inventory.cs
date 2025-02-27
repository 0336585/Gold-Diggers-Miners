using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [SerializeField] private GameObject inventoryItemHolder;
    [SerializeField] private GameObject inventoryItem;

    // Dictionary to store minerals and their quantities
    private Dictionary<InventoryMineral, int> minerals = new Dictionary<InventoryMineral, int>();

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
    public void AddMineral(InventoryMineral mineral, int amount)
    {
        if (minerals.ContainsKey(mineral))
        {
            minerals[mineral] += amount;
        }
        else
        {
            minerals.Add(mineral, amount);
        }

        Debug.Log(mineral.mineralName + " added. Current amount: " + minerals[mineral]);
    }

    // Method to remove minerals from the inventory
    public void RemoveMineral(InventoryMineral mineral, int amount)
    {
        if (minerals.ContainsKey(mineral) && minerals[mineral] >= amount)
        {
            minerals[mineral] -= amount;
            Debug.Log(mineral + " removed. Current amount: " + minerals[mineral]);
        }
        else
        {
            Debug.Log("Not enough " + mineral.mineralName + " to remove.");
        }
    }

    // Method to check the amount of a mineral in the inventory
    public int GetMineralAmount(InventoryMineral mineral)
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenInventory();
        }
    }

    private void OpenInventory()
    {
        Debug.Log("Opening Inventory...");

        // Loop through the minerals dictionary
        foreach (KeyValuePair<InventoryMineral, int> entry in minerals)
        {
            InventoryMineral mineral = entry.Key;
            int amount = entry.Value;

            // Log the mineral name and its quantity
            Debug.Log("Mineral: " + mineral.mineralName + ", Amount: " + amount);

            // Optionally, instantiate UI elements to display the inventory
            if (inventoryItemHolder != null)
            {
                // Create a new UI element for each mineral
                GameObject itemUI = Instantiate(inventoryItem);
                itemUI.transform.SetParent(inventoryItemHolder.transform);

                itemUI.GetComponent<InventorySlot>().SetMineralSlot(mineral, amount);
            }
        }
    }
}
