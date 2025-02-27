using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    private GameObject equipedItemGO;
    private InventoryItem equipedItem;
    public InventoryItem EquipedItem
    {
        get { return equipedItem; }
        private set { equipedItem = value; }
    }
    private PlayerMining playerMining;
    private WeaponUIManager weaponUIManager;

    [SerializeField] private List<InventoryItem> hotbarItems = new List<InventoryItem>();
    [SerializeField] private List<Image> hotbarSlots;

    [Header("References")]
    [SerializeField] private GameObject itemHolder;
    [SerializeField] private GameObject hotbarUI;

    [Header("Items")]
    [SerializeField] private GameObject pickaxe;
    [SerializeField] private GameObject revolver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMining = GetComponent<PlayerMining>();
        weaponUIManager = GetComponent<WeaponUIManager>(); 

        for (int i = 0; i < hotbarUI.transform.childCount; i++)
        {
            hotbarSlots.Add(hotbarUI.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>());
        }

        EquipItem(hotbarItems[0]);

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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (hotbarItems[0] != null)
                EquipItem(hotbarItems[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (hotbarItems[1] != null)
                EquipItem(hotbarItems[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (hotbarItems[2] != null)
                EquipItem(hotbarItems[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (hotbarItems[3] != null)
                EquipItem(hotbarItems[3]);
        }
    }

    private void EquipItem(InventoryItem _item)
    {
        if (equipedItemGO == _item) return;

        if (equipedItemGO)
            Destroy(equipedItemGO);

        if (_item.itemType == ItemType.Tool)
            playerMining.EnableMining(true);
        else
            playerMining.EnableMining(false);


        equipedItemGO = Instantiate(_item.prefab, itemHolder.transform);
        equipedItem = _item;

        weaponUIManager.ChangeToolUI(equipedItem);
    }
}
