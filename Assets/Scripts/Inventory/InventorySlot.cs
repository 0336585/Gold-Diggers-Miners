using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    private Material rarityMat;
    private Image icon;
    private Image background;
    private TextMeshProUGUI stackText;

    private InventoryUIManager inventoryUIManager;

    private void Awake()
    {
        icon = gameObject.transform.GetChild(0).GetComponent<Image>();
        stackText = GetComponentInChildren<TextMeshProUGUI>();
        background = GetComponent<Image>();

        inventoryUIManager = gameObject.transform.parent.transform.parent.GetComponentInParent<InventoryUIManager>();
    }

    public void SetMineralSlot(InventoryMineral _mineral, int _amount)
    {
        switch (_mineral.rarity)
        {
            case MineralRarity.uncommon:
                rarityMat = inventoryUIManager.UncommonMat;
                break;
            case MineralRarity.rare:
                rarityMat = inventoryUIManager.RareMat;
                break;
            case MineralRarity.epic:
                rarityMat = inventoryUIManager.EpicMat;
                break;
            case MineralRarity.legendary:
                rarityMat = inventoryUIManager.LegendaryMat;
                break;
            case MineralRarity.mythic:
                rarityMat = inventoryUIManager.MythicMat;
                break;
            case MineralRarity.secret:
                rarityMat = inventoryUIManager.SecretMat;
                break;
        }

        icon.sprite = _mineral.icon;
        stackText.text = _amount.ToString();
        background.material = rarityMat;
    }


}
