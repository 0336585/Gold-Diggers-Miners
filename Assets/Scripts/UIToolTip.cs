using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltipPrefab;
    private GameObject tooltip;
    private RectTransform tooltipRect;

    private InventoryMineral mineral;
    private TextMeshProUGUI rarityText;
    private TextMeshProUGUI itemNameText;
    private TextMeshProUGUI sellPriceText;
    private TextMeshProUGUI maxStackText;

    private void Update()
    {
        if (tooltip != null)
        {
            // Set tooltip position to mouse position (top-left aligned)
            Vector2 mousePosition = Input.mousePosition;
            tooltipRect.position = new Vector2(mousePosition.x + 20, mousePosition.y);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip == null)
        {
            tooltip = Instantiate(tooltipPrefab, transform.root); // Instantiate under Canvas
            tooltipRect = tooltip.GetComponent<RectTransform>();
        }

        tooltip.SetActive(true);

        mineral = GetComponent<InventorySlot>().Mineral;

        // Cache the tooltip's child container
        Transform tooltipContent = tooltip.transform.GetChild(0);

        // Find text components inside the tooltip prefab
        rarityText = tooltipContent.Find("RarityText").GetComponent<TextMeshProUGUI>();
        itemNameText = tooltipContent.Find("ItemNameText").GetComponent<TextMeshProUGUI>();
        sellPriceText = tooltipContent.Find("SellPriceText").GetComponent<TextMeshProUGUI>();
        maxStackText = tooltipContent.Find("MaxStackText").GetComponent<TextMeshProUGUI>();

        // Assign values
        rarityText.text = mineral.rarity.ToString();
        itemNameText.text = mineral.mineralName;
        sellPriceText.text = "Sell price: " + mineral.sellPrice;
        maxStackText.text = "Max stack: " + mineral.maxStack;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            Destroy(tooltip);
            tooltip = null;
        }
    }
}
