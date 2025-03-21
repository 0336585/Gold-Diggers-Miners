using UnityEngine;
using TMPro;

public class QuotaManager : MonoBehaviour
{
    public static QuotaManager Instance;

    [Header("Win Condition")]
    [SerializeField] private InventoryMineral mineral1;
    [SerializeField] private int mineral1AmountNeeded;
    private int mineral1CurrentAmount;
    [SerializeField] private InventoryMineral mineral2;
    [SerializeField] private int mineral2AmountNeeded;
    private int mineral2CurrentAmount;
    [SerializeField] private InventoryMineral mineral3;
    [SerializeField] private int mineral3AmountNeeded;
    private int mineral3CurrentAmount;

    [Header("Sleep Quota Info")]
    [SerializeField] private int spinsToReachSleepQuota;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI sleepQuotaText;
    [SerializeField] private TextMeshProUGUI winQuotaText;
    [SerializeField] private GameObject winScreen;

    private int spinsAmount;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate inventory instances
            return;
        }

        Instance = this;

        //Debug.Log("Spins to reach quota: " + spinsToReachSleepQuota);
        sleepQuotaText.text = $"Casino spins: {spinsAmount}/{spinsToReachSleepQuota}";
    }

    private void Start()
    {
        SetWinQuotaText();
    }

    private void SetWinQuotaText()
    {
        winQuotaText.text = $"{mineral1.mineralName}: {mineral1CurrentAmount}/{mineral1AmountNeeded}\n" +
                            $"{mineral2.mineralName}: {mineral2CurrentAmount}/{mineral2AmountNeeded}\n" +
                            $"{mineral3.mineralName}: {mineral3CurrentAmount}/{mineral3AmountNeeded}";
    }

    public void AddSpin()
    {
        spinsAmount++;
        sleepQuotaText.text = $"Casino spins: {spinsAmount}/{spinsToReachSleepQuota}";
    }

    public bool PlayerReachedQuota()
    {
        if (spinsAmount >= spinsToReachSleepQuota)
        {
            spinsAmount = 0;
            sleepQuotaText.text = $"Casino spins: {spinsAmount}/{spinsToReachSleepQuota}";
            return true;
        }

        return false;
    }

    public void AddMineral()
    {

        mineral1CurrentAmount += Inventory.Instance.GetMineralAmount(mineral1);

        if (Inventory.Instance.GetMineralAmount(mineral1) < (mineral1AmountNeeded - mineral1CurrentAmount))
        {
            Inventory.Instance.RemoveMineral(mineral1, Inventory.Instance.GetMineralAmount(mineral1));
        }
        else
        {
            mineral1CurrentAmount = mineral1AmountNeeded;
            Inventory.Instance.RemoveMineral(mineral1, mineral1AmountNeeded);
        }


        mineral2CurrentAmount += Inventory.Instance.GetMineralAmount(mineral2);

        if (Inventory.Instance.GetMineralAmount(mineral2) < (mineral2AmountNeeded - mineral2CurrentAmount))
        {
            Inventory.Instance.RemoveMineral(mineral2, Inventory.Instance.GetMineralAmount(mineral2));
        }
        else
        {
            mineral2CurrentAmount = mineral2AmountNeeded;
            Inventory.Instance.RemoveMineral(mineral2, mineral2AmountNeeded);
        }

        mineral3CurrentAmount += Inventory.Instance.GetMineralAmount(mineral3);

        if (Inventory.Instance.GetMineralAmount(mineral3) < (mineral3AmountNeeded - mineral3CurrentAmount))
        {
            Inventory.Instance.RemoveMineral(mineral3, Inventory.Instance.GetMineralAmount(mineral3));
        }
        else
        {
            mineral3CurrentAmount = mineral3AmountNeeded;
            Inventory.Instance.RemoveMineral(mineral3, mineral3AmountNeeded);
        }


        SetWinQuotaText();

    }

    private bool PlayerReachedWinQuota()
    {
        if (mineral1CurrentAmount >= mineral1AmountNeeded && mineral2CurrentAmount >= mineral2AmountNeeded && mineral3CurrentAmount >= mineral3AmountNeeded)
            return true;

        return false;
    }

    public void ShowWinScreen()
    {
        if (PlayerReachedQuota())
            winScreen.SetActive(true);
    }
}
