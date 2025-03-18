using TMPro;
using UnityEngine;

public class WeaponUIManager : MonoBehaviour
{
    // TEHCNICALLY THIS SCRIPT NEEDS TO BE REWRITTEN WITH THE FUNCTIONALITY OF WEAPONS, THROUGH THE HOTBAR
    [SerializeField] private GameObject reloadText;
    [SerializeField] private GameObject ammoGO;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private Hotbar hotbar;
    private GameObject weapon;
    private ProjectileShooter projectileShooter;
    private ProjectileThrowing projectileThrowing;
    void Start()
    {
        hotbar = GetComponent<Hotbar>();
        reloadText.SetActive(false);
    }
    void Update()
    {
        //If weapon is reloading
        ShowReloadingText();
        UpdateAmmoText();
    }
    public void ShowReloadingText()
    {
        reloadText.SetActive(projectileShooter.IsReloading);
    }
    public void ChangeToolUI(InventoryItem item)
    {
        if(item.itemType == ItemType.RangedWeapon)
        {
            ammoGO.SetActive(true);
            projectileShooter = GetComponentInChildren<ProjectileShooter>();
            //Debug.Log("Ik ben het wapen: " + projectileShooter.transform.name);
        }
        else if(item.itemType == ItemType.Throwable)
        {
            ammoGO.SetActive(true);
            projectileThrowing = GetComponentInChildren<ProjectileThrowing>();
        }
        else
        {
            ammoGO.SetActive(false);
        }
    }
    private void UpdateAmmoText()
    {

        if(hotbar.EquipedItem.itemType == ItemType.RangedWeapon)
        {
            ammoText.text = $"{projectileShooter.CurrentAmmo}|{projectileShooter.ReserveAmmo}";
        }
        if (hotbar.EquipedItem.itemType == ItemType.Throwable)
        {
            ammoText.text = $"{projectileThrowing.CurrentAmmo}";
        }

    }
}
