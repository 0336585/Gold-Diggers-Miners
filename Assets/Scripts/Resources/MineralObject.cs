using UnityEngine;

public class MineralObject : MonoBehaviour
{
    [Header("Mineral Settings")]
    [SerializeField] private InventoryMineral mineralType;

    private void OnDestroy()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.AddMineral(mineralType, mineralType.dropAmount);
            Debug.Log($"{mineralType.dropAmount} {mineralType.mineralType} added to inventory.");
        }
    }
}
