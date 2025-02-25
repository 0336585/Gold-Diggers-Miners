using UnityEngine;

public class Hotbar : MonoBehaviour
{
    private GameObject equipedItem;
    private PlayerMining playerMining;

    [Header("References")]
    [SerializeField] private GameObject itemHolder;

    [Header("Items")]
    [SerializeField] private GameObject pickaxe;
    [SerializeField] private GameObject revolver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMining = GetComponent<PlayerMining>();

        EquipItem(pickaxe);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipItem(pickaxe);
            playerMining.EnableMining(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipItem(revolver);
            playerMining.EnableMining(false);
        }
    }

    private void EquipItem(GameObject _item)
    {
        if (equipedItem == _item) return;

        if (equipedItem)
            Destroy(equipedItem);

        equipedItem = Instantiate(_item, itemHolder.transform);


    }
}
