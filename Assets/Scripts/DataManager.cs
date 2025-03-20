using System.IO;
using UnityEngine;

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

            if(moneyManager != null && sleepingBehaviour != null)
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
            Debug.LogWarning("No save file found!");
        }
    }
}
