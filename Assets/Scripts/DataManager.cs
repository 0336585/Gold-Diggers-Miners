using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public uint days;
    public int money;
    public List<InventoryItem> hotbarItems = new List<InventoryItem>();
}

public class DataManager : MonoBehaviour
{
    public SaveData saveData = new SaveData();
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private SleepingBehaviour sleepingBehaviour;
    [SerializeField] private Hotbar hotbar;

    private string filePath;
    [SerializeField] private List<InventoryItem> defaultHotbarItems = new List<InventoryItem>();

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        LoadData();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Data Saved: " + json);

        saveData.hotbarItems = hotbar.HotbarItems;
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            saveData = JsonUtility.FromJson<SaveData>(json);

            // Handle days and money
            if (moneyManager != null && sleepingBehaviour != null)
            {
                moneyManager.Money = saveData.money;
                sleepingBehaviour.SurvivedDaysAmount = saveData.days;
            }
            else
            {
                Debug.Log("Note: Can't set days and money data");
            }

            // Handle hotbar items
            if (hotbar != null)
            {
                if (saveData.hotbarItems == null || saveData.hotbarItems.Count == 0)
                {
                    saveData.hotbarItems = new List<InventoryItem>(defaultHotbarItems);
                    Debug.Log("No weapons found in save data. Using default weapons.");
                }

                hotbar.HotbarItems = new List<InventoryItem>(saveData.hotbarItems);
            }

            Debug.Log("Data Loaded: " + json);
        }
        else
        {
            Debug.LogWarning("No save file found! Creating a new save file with default values.");

            saveData = new SaveData
            {
                money = 0,
                days = 0,
                hotbarItems = new List<InventoryItem>(defaultHotbarItems)
            };

            SaveData();
        }
    }
}
