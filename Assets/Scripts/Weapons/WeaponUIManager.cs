using TMPro;
using UnityEngine;

public class WeaponUIManager : MonoBehaviour
{
    [SerializeField] private GameObject reloadText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private GameObject weapon;
    private ProjectileShooter projectileShooter;
    void Start()
    {
        projectileShooter = weapon.GetComponent<ProjectileShooter>();
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
    private void UpdateAmmoText()
    {
        ammoText.text = $"{projectileShooter.CurrentAmmo}|{projectileShooter.ReserveAmmo}";
    }
}
