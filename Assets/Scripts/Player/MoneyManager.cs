using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private DataManager saveDataManager;

    public static MoneyManager Instance;

    private int money;

    public int Money
    {
        get { return money; }
        set { money = value; moneyText.text = money.ToString(); }
    }

    [SerializeField] private TextMeshProUGUI moneyText;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate inventory instances
            return;
        }

        Instance = this;
        moneyText.text = money.ToString();
    }

    public void AddMoney(int _amount)
    {
        money += _amount;
        saveDataManager.saveData.money = money;
        moneyText.text = money.ToString();
    }

    public void RemoveMoney(int _amount)
    {
        money -= _amount;
        saveDataManager.saveData.money = money;
        moneyText.text = money.ToString();
    }
}
