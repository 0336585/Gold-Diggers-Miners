using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public uint days;
    public int money;
}

public class DataManager : MonoBehaviour
{
    public SaveData saveData = new SaveData();
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private SleepingBehaviour sleepingBehaviour;
    [SerializeField] private Hotbar hotbar;

    private string filePath;

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
            }
            else
            {
                Debug.Log("Note: Can't set days and money data");
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
            };

            // Save the new file
            SaveData();
        }
    }
}
