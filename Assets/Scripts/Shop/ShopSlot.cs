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

        parent.transform.Find("Sell_1_Button").GetComponent<Button>().onClick.AddListener(OnSell);
        parent.transform.Find("Sell_All_Button").GetComponent<Button>().onClick.AddListener(OnSellAll);
    }

    private void Update()
    {
        rarityMat?.SetFloat("_UnscaledTime", Time.unscaledTime);
    }

    public void SetImage(InventoryMineral _mineral)
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
        background.material = rarityMat;
        mineral = _mineral;
    }

    public void SetContent(InventoryMineral _mineral)
    {
        TextMeshProUGUI nameText = parent.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI priceText = parent.transform.Find("Price").GetComponent<TextMeshProUGUI>();

        nameText.text = _mineral.mineralName;
        priceText.text = "Price: " + _mineral.sellPrice.ToString();
        mineral = _mineral;
    }

    public void OnSell()
    {
        if (Inventory.Instance.GetMineralAmount(mineral) <= 0) return;

        MoneyManager.Instance.AddMoney(mineral.sellPrice);
        Inventory.Instance.RemoveMineral(mineral, 1);
    }

    public void OnSellAll()
    {
        if (Inventory.Instance.GetMineralAmount(mineral) <= 0) return;

        for (int i = 0; i < Inventory.Instance.GetMineralAmount(mineral); i++)
        {
            MoneyManager.Instance.AddMoney(mineral.sellPrice);
        }

        Inventory.Instance.RemoveMineral(mineral, Inventory.Instance.GetMineralAmount(mineral));
    }
}
