using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Header("References")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject inventoryItemHolder;
    [SerializeField] private GameObject inventoryItem;
    private GameObject currentHover;

    // Dictionary to store minerals and their quantities
    private Dictionary<InventoryMineral, int> minerals = new Dictionary<InventoryMineral, int>();
    private List<GameObject> itemsShowing = new List<GameObject> ();

    private bool inventoryIsOpen = false;

    private void Start()
    {
        MenuManager.Instance.OnMenuOpen += CloseInventory;

    }

    private void OnDisable()
    {
        MenuManager.Instance.OnMenuOpen -= CloseInventory;
    }

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
        if (minerals.ContainsKey(mineral) && (minerals[mineral] + amount) >= mineral.maxStack)
        {
            minerals[mineral] = mineral.maxStack;
            return;
        }

        if (minerals.ContainsKey(mineral))
        {
            minerals[mineral] += amount;
        }
        else
        {
            minerals.Add(mineral, amount);
        }

        //Debug.Log(mineral.mineralName + " added. Current amount: " + minerals[mineral]);
    }

    // Method to remove minerals from the inventory
    public void RemoveMineral(InventoryMineral mineral, int amount)
    {
        if (minerals.ContainsKey(mineral) && minerals[mineral] >= amount)
        {
            minerals[mineral] -= amount;
            //Debug.Log(mineral + " removed. Current amount: " + minerals[mineral]);
        }
        else
        {
            //Debug.Log("Not enough " + mineral.mineralName + " to remove.");
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
            if (!inventoryIsOpen)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    private void OpenInventory()
    {
        if (inventoryIsOpen) return;

        //Debug.Log("Opening Inventory...");

        inventoryIsOpen = true;
        MenuManager.Instance.MenuEvent();
        inventory.SetActive(true);

        // Loop through the minerals dictionary
        foreach (KeyValuePair<InventoryMineral, int> entry in minerals)
        {
            InventoryMineral mineral = entry.Key;
            int amount = entry.Value;

            // Log the mineral name and its quantity
            //Debug.Log("Mineral: " + mineral.mineralName + ", Amount: " + amount);

            // Optionally, instantiate UI elements to display the inventory
            if (inventoryItemHolder != null)
            {
                // Create a new UI element for each mineral
                GameObject itemUI = Instantiate(inventoryItem, inventoryItemHolder.transform);

                itemUI.GetComponent<InventorySlot>().SetMineralSlot(mineral, amount);
            }
        }
    }

    public void CloseInventory()
    {
        inventoryIsOpen = false;
        inventory.SetActive(false);
        MenuManager.Instance.MenuEventClosed();


        Destroy(currentHover);

        // Get all the InventorySlot components in the children of inventoryItemHolder
        InventorySlot[] showingGO = inventoryItemHolder.GetComponentsInChildren<InventorySlot>();

        // Loop through and destroy each game object
        for (int i = 0; i < showingGO.Length; i++)
        {
            Destroy(showingGO[i].gameObject);
        }
    }

    public void SetCurrentHover(GameObject _currentHover) => currentHover = _currentHover;
}
