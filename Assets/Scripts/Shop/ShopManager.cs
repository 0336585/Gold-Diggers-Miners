using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<InventoryMineral> minerals;

    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject itemHolder;
    [SerializeField] private GameObject shopItem;

    private bool shopIsOpen = false;

    public void OpenShop()
    {
        if (shopIsOpen) return;

        shopIsOpen = true;
        shop.SetActive(true);

        // Loop through the minerals array
        foreach (InventoryMineral _mineral in minerals)
        {
            InventoryMineral mineral = _mineral;

            // Optionally, instantiate UI elements to display the items
            if (itemHolder != null)
            {
                // Create a new UI element for each mineral
                GameObject itemUI = Instantiate(shopItem, itemHolder.transform);
                ShopSlot shopSlot = itemUI.GetComponentInChildren<ShopSlot>();
                shopSlot.SetImage(mineral);
                shopSlot.SetContent(mineral);
            }
        }
    }

    public void CloseShop()
    {
        if (!shopIsOpen) return;

        shopIsOpen = false;

        // Get all the ShopSlot components in the children of itemHolder
        ShopSlot[] showingGO = itemHolder.GetComponentsInChildren<ShopSlot>();

        // Loop through and destroy each game object
        for (int i = 0; i < showingGO.Length; i++)
        {
            Destroy(showingGO[i].transform.parent.gameObject);
        }

        shop.SetActive(false);
    }
}
