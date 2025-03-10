using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    private int money;

    public int Money
    {
        get { return money; }
        private set { money = value; }
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

        money = 0;
        moneyText.text = money.ToString();
    }

    public void AddMoney(int _amount)
    {
        money += _amount;
        moneyText.text = money.ToString();
    }
    public void RemoveMoney(int _amount)
    {
        money -= _amount;
        moneyText.text = money.ToString();
    }
}
