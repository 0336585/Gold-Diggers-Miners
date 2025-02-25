using UnityEngine;

public class WeaponUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _reloadText;
    [SerializeField] private GameObject _weapon;
    private ProjectileShooter _projectileShooter;
    void Start()
    {
        _projectileShooter = _weapon.GetComponent<ProjectileShooter>();
        _reloadText.SetActive(false);
    }
    void Update()
    {
        //If weapon is reloading
        ShowReloadingText();
    }
    public void ShowReloadingText()
    {
        _reloadText.SetActive(true);
    }
}
