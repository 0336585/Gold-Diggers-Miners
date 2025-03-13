using TMPro;
using UnityEngine;
using System.Collections;

public class SleepingBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text daysUI;
    [SerializeField] private GameObject sleepMenu;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private OxygenManager oxygenManager;
    private uint survivedDaysAmount; 
    private AudioSource sleepSFX;
    private Animator menuAnimator;



    private bool playerIsSleeping = false;

    private void Start()
    {
        sleepSFX = GetComponent<AudioSource>();
    }

    public void Sleep()
    {
        if (QuotaManager.Instance.PlayerReachedQuota())
        {
            if (playerIsSleeping) return;

            playerIsSleeping = true;
            // TODO: Make fade in fade out
            survivedDaysAmount++;
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
            // TODO: Add more conditions to this, for gambling and quota
            daysUI.text = $"Can't sleep just yet...";
        }

        
    }

    private IEnumerator TimeToSleepAgain(float _time)
    {
        yield return new WaitForSeconds(_time);
        playerIsSleeping = false;
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
