using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    private GameObject equipedItemGO;
    public InventoryItem equipedItem;

    public InventoryItem EquipedItem
    {
        get { return equipedItem; }
        private set { equipedItem = value; }
    }

    private PlayerMining playerMining;
    private WeaponUIManager weaponUIManager;
    private ProjectileShooter projectileShooter;

    [SerializeField] private List<InventoryItem> hotbarItems = new List<InventoryItem>();

    public List<InventoryItem> HotbarItems
    {
        get { return hotbarItems; }
        set { hotbarItems = value; }
    }

    [SerializeField] private List<Image> hotbarSlots;

    [Header("References")]
    [SerializeField] private GameObject itemHolder;
    [SerializeField] private GameObject hotbarUI;

    private List<GameObject> instantiatedItems = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMining = GetComponent<PlayerMining>();
        weaponUIManager = GetComponent<WeaponUIManager>();
        projectileShooter = GetComponentInChildren<ProjectileShooter>();

        for (int i = 0; i < hotbarUI.transform.childCount; i++)
        {
            hotbarSlots.Add(hotbarUI.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>());
        }

        // Instantiate all items in the hotbar and store them in a list
        foreach (var item in hotbarItems)
        {
            if (item != null)
            {
                GameObject instantiatedItem = Instantiate(item.prefab, itemHolder.transform);
                instantiatedItem.SetActive(false); // Deactivate all items initially
                instantiatedItems.Add(instantiatedItem);
            }
        }

        // Equip the first item in the list
        if (hotbarItems.Count > 0 && hotbarItems[0] != null)
        {
            EquipItem(hotbarItems[0]);
        }

        // Update the hotbar UI
        for (int i = 0; i < hotbarItems.Count; i++)
        {
            if (hotbarItems[i] == null) break;

            if (hotbarSlots[i].transform.name.Contains("Item"))
                hotbarSlots[i].sprite = hotbarItems[i].icon;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MenuManager.Instance.inMenu) return;

        weaponUIManager.ChangeToolUI(equipedItem);

        if (Input.GetKeyDown(KeyCode.Alpha1) && !projectileShooter.IsReloading)
        {
            if (hotbarItems[0] != null)
                EquipItem(hotbarItems[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !projectileShooter.IsReloading)
        {
            if (hotbarItems[1] != null)
                EquipItem(hotbarItems[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !projectileShooter.IsReloading)
        {
            if (hotbarItems[2] != null)
                EquipItem(hotbarItems[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && !projectileShooter.IsReloading)
        {
            if (hotbarItems[3] != null)
                EquipItem(hotbarItems[3]);
        }
    }

    public void EquipItem(InventoryItem _item)
    {
        if (equipedItem == _item) return;

        // Deactivate all items
        foreach (var item in instantiatedItems)
            item.SetActive(false);

        // Activate the equipped item
        int index = hotbarItems.IndexOf(_item);
        if (index != -1 && index < instantiatedItems.Count)
        {
            instantiatedItems[index].SetActive(true);
            equipedItemGO = instantiatedItems[index];
            equipedItem = _item;
        }

        // Update mining ability based on item type
        if (_item.itemType == ItemType.Tool)
        {
            playerMining.EnableMining(true);
        }
        else
        {
            playerMining.EnableMining(false);
            playerMining.RemoveOutline();
        }

        if (_item.itemType == ItemType.RangedWeapon)
            projectileShooter = GetComponentInChildren<ProjectileShooter>();

        weaponUIManager.ChangeToolUI(equipedItem);

        //for (int i = 0; i < hotbarItems.Count; i++)
        //{
        //    if (_item == hotbarItems[i]) return;
        //}
    }

    public void AddItemToHotbar(InventoryItem _item)
    {
        // Check if the exact same item is already in the hotbar
        for (int i = 0; i < hotbarItems.Count; i++)
        {
            if (hotbarItems[i] == _item) // If the same item is already in the hotbar, do nothing
            {
                return;
            }
        }

        // Check if an item of the same type exists in the hotbar
        for (int i = 0; i < hotbarItems.Count; i++)
        {
            if (hotbarItems[i] != null && hotbarItems[i].itemType == _item.itemType)
            {
                // Replace the old item in the hotbar
                hotbarItems[i] = _item;

                // Destroy the old instantiated item
                if (instantiatedItems[i] != null)
                {
                    Destroy(instantiatedItems[i]);
                }

                // Instantiate the new item at the correct position
                GameObject instantiatedItem = Instantiate(_item.prefab, itemHolder.transform);
                instantiatedItem.SetActive(false); // Deactivate initially
                instantiatedItems[i] = instantiatedItem; // Replace in list

                // Update the hotbar UI slot
                if (hotbarSlots[i].transform.name.Contains("Item"))
                    hotbarSlots[i].sprite = _item.icon;

                return; // Exit function after replacing the item
            }
        }

        // If no existing item of the same type was found, add the new item (if there is space)
        if (hotbarItems.Count < hotbarSlots.Count)
        {
            hotbarItems.Add(_item);

            // Instantiate the new item and add it to the list
            GameObject instantiatedItem = Instantiate(_item.prefab, itemHolder.transform);
            instantiatedItem.SetActive(false);
            instantiatedItems.Add(instantiatedItem);

            // Update UI slot
            int newIndex = hotbarItems.Count - 1;
            if (hotbarSlots[newIndex].transform.name.Contains("Item"))
                hotbarSlots[newIndex].sprite = _item.icon;
        }
    }



}