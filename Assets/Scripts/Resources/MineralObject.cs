using UnityEngine;

public class MineralObject : MonoBehaviour
{
    [Header("Mineral Settings")]
    [SerializeField] private MineralType mineralType;
    [SerializeField] private int dropAmount = 1;  

    private void OnDestroy()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.AddMineral(mineralType, dropAmount);
            Debug.Log($"{dropAmount} {mineralType} added to inventory.");
        }
        else
        {
            Debug.LogError("Inventory instance not found!");
        }
    }
}
