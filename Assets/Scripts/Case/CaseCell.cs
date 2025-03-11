using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaseCell : MonoBehaviour
{
    [System.Serializable]
    private class ListOfItems
    {
        public List<InventoryItem> inventoryItem;
    }

    [SerializeField] private List<ListOfItems> items;

    [SerializeField] private int[] chances;

    [SerializeField] private Material[] rarityMaterials;

    public InventoryItem itemInThisCell;

    public void Setup()
    {
        int index = Randomize();

        itemInThisCell = items[index].inventoryItem[Random.Range(0, items[index].inventoryItem.Count)];

        // Assigning the icon of the selected character to the Image component
        GetComponent<Image>().sprite = itemInThisCell.icon;
        transform.parent.GetComponent<Image>().material = rarityMaterials[index];
        transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = itemInThisCell.itemName;
    }

    private int Randomize()
    {
        int totalChance = 0;

        // Calculate total chance sum
        foreach (int chance in chances)
        {
            totalChance += chance;
        }

        int rnd = Random.Range(0, totalChance);
        int cumulative = 0;

        for (int i = 0; i < chances.Length; i++)
        {
            cumulative += chances[i];
            if (rnd < cumulative)
            {
                return i;
            }
        }

        return chances.Length - 1; // Default return to avoid errors
    }

}

