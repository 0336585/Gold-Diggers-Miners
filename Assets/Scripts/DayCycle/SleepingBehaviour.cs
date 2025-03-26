using TMPro;
using UnityEngine;
using System.Collections;

public class SleepingBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text daysUI;
    [SerializeField] private TMP_Text addictUI;
    [SerializeField] private GameObject sleepMenu;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private OxygenManager oxygenManager;
    [SerializeField] private DataManager saveDataManager;

    private uint survivedDaysAmount; // uInt can't get in the negative
    private AudioSource sleepSFX;
    private Animator menuAnimator;
    private bool playerIsSleeping = false;

    public uint SurvivedDaysAmount
    {
        get { return survivedDaysAmount; }
        set { survivedDaysAmount = value; }
    }

    private void Start()
    {
        sleepSFX = GetComponent<AudioSource>();
        survivedDaysAmount = saveDataManager.saveData.days;
    }

    public void Sleep()
    {
        if (QuotaManager.Instance.PlayerReachedQuota())
        {
            if (playerIsSleeping) return;

            playerIsSleeping = true;

            saveDataManager.saveData.days = survivedDaysAmount;
            survivedDaysAmount++;
            saveDataManager.SaveData();

            sleepSFX.Play();

            if (survivedDaysAmount == 1)
            {
                daysUI.text = $"you have survived {survivedDaysAmount} day";
            }
            else
            {
                daysUI.text = $"you have survived {survivedDaysAmount} days";
            }

            sleepMenu.SetActive(true);
            menuAnimator = sleepMenu.GetComponent<Animator>();
            menuAnimator.SetTrigger("SleepAnim");

            StartCoroutine(TimeToSleepAgain(6));
        }
        else
        {
            addictUI.gameObject.SetActive(true);
            addictUI.text = "I need to Gamble...";
            StartCoroutine(HideTextAfterDelay(2)); 
        }
    }

    private IEnumerator TimeToSleepAgain(float _time)
    {
        yield return new WaitForSeconds(_time);
        playerIsSleeping = false;
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        addictUI.gameObject.SetActive(false);
    }

    public void SleepEvent()
    {
        levelGenerator.RegenerateLevel();
        playerHealth.SetMaxHealth();
        oxygenManager.AddMaxOxygen();
        playerIsSleeping = false;
        sleepMenu.SetActive(false);
    }
}
