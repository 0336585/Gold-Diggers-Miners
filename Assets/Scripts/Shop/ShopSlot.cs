using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    private GameObject parent;

    private Material rarityMat;
    private Image icon;
    private Image background;

    private InventoryUIManager inventoryUIManager;

    private InventoryMineral mineral;

    public InventoryMineral Mineral
    {
        get { return mineral; }
        private set { mineral = value; }
    }

    private void Awake()
    {
        icon = gameObject.transform.GetChild(0).GetComponent<Image>();
        background = GetComponent<Image>();

        inventoryUIManager = InventoryUIManager.Instance;
        parent = transform.parent.gameObject;
    }

    public void SetImage(InventoryMineral _mineral)
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
        background.material = rarityMat;
        mineral = _mineral;
    }

    public void SetContent(InventoryMineral _mineral)
    {
        TextMeshProUGUI nameText = parent.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI priceText = parent.transform.Find("Price").GetComponent<TextMeshProUGUI>();

        nameText.text = _mineral.mineralName;
        priceText.text = "Price: " + _mineral.sellPrice.ToString();
    }
}
