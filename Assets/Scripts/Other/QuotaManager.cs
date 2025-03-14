using UnityEngine;
using TMPro;

public class QuotaManager : MonoBehaviour
{
    public static QuotaManager Instance;

    [Header("Quota info")]
    [SerializeField] private int spinsToReachQuota;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI quotaText;

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
        quotaText.text = $"Casino spins: {spinsAmount}/{spinsToReachQuota}";
    }

    public void AddSpin()
    {
        spinsAmount++;
        quotaText.text = $"Casino spins: {spinsAmount}/{spinsToReachQuota}";
    }

    public bool PlayerReachedQuota()
    {
        if (spinsAmount >= spinsToReachQuota)
        {
            spinsAmount = 0;
            quotaText.text = $"Casino spins: {spinsAmount}/{spinsToReachQuota}";
            return true;
        }

        return false;
    }
}
