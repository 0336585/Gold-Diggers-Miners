using UnityEngine;

public class WeaponUIManager : MonoBehaviour
{
    [SerializeField] private GameObject reloadText;
    [SerializeField] private GameObject weapon;
    private ProjectileShooter _projectileShooter;
    void Start()
    {
        _projectileShooter = weapon.GetComponent<ProjectileShooter>();
        reloadText.SetActive(false);
    }
    void Update()
    {
        //If weapon is reloading
        ShowReloadingText();
    }
    public void ShowReloadingText()
    {
        reloadText.SetActive(true);
    }
}
