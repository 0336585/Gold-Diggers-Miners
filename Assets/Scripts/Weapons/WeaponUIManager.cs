using UnityEngine;

public class WeaponUIManager : MonoBehaviour
{
    [SerializeField] private GameObject reloadText;
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
    }
    public void ShowReloadingText()
    {
        reloadText.SetActive(projectileShooter.IsReloading);
    }
}
