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
    private List<InventoryItem> defaultHotbarItems = new List<InventoryItem>();

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

            if (moneyManager != null && sleepingBehaviour != null && hotbar != null)
            {
                moneyManager.Money = saveData.money;
                sleepingBehaviour.SurvivedDaysAmount = saveData.days;
                hotbar.HotbarItems = saveData.hotbarItems;
            }
            else
            {
                Debug.Log("Note: Can't set days and money data");
            }

            // Check if hotbarItems is empty and replace with default values if needed
            if (saveData.hotbarItems == null || saveData.hotbarItems.Count == 0)
            {
                saveData.hotbarItems = defaultHotbarItems;
                Debug.Log("No weapons found in save data. Using default weapons.");
            }

            Debug.Log("Data Loaded: " + json);
        }
        else
        {
            Debug.LogWarning("No save file found! Creating a new save file with default values.");

            // Initialize saveData with default values
            saveData = new SaveData
            {
                money = 0,
                days = 0,
                hotbarItems = defaultHotbarItems
            };

            // Save the new file
            SaveData();
        }
    }

    public void FillDefaultHotbarItems(List<InventoryItem> _defaultHotbarItems)
    {
        defaultHotbarItems = _defaultHotbarItems;
    }
}
