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

    private InventoryMineral mineral;

    public InventoryMineral Mineral
    {
        get { return mineral;  }
        private set { mineral = value;  }
    }

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
            case MineralRarity.Uncommon:
                rarityMat = inventoryUIManager.UncommonMat;
                break;
            case MineralRarity.Rare:
                rarityMat = inventoryUIManager.RareMat;
                break;
            case MineralRarity.Epic:
                rarityMat = inventoryUIManager.EpicMat;
                break;
            case MineralRarity.Legendary:
                rarityMat = inventoryUIManager.LegendaryMat;
                break;
            case MineralRarity.Mythic:
                rarityMat = inventoryUIManager.MythicMat;
                break;
            case MineralRarity.Secret:
                rarityMat = inventoryUIManager.SecretMat;
                break;
        }

        icon.sprite = _mineral.icon;
        stackText.text = _amount.ToString();
        background.material = rarityMat;
        mineral = _mineral;
    }


}
