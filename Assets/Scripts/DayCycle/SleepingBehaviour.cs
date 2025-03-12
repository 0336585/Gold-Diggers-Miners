using TMPro;
using UnityEngine;
using System.Collections;

public class SleepingBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text daysUI;
    [SerializeField] private float textShowupTime = 2f; //TODO: Show text is inregular speed when spam clicking
    [SerializeField] private PlayerHealth playerHealth;
    public bool canSleep = true;
    private uint survivedDaysAmount; 
    private AudioSource sleepSFX;

    private void Start()
    {
        sleepSFX = GetComponent<AudioSource>();
    }

    public void Sleep()
    {
        if (canSleep)
        {
            survivedDaysAmount++;
            canSleep = false;
            sleepSFX.Play();
            playerHealth.SetMaxHealth();

            if (survivedDaysAmount == 1)
            {
                daysUI.text = $"you have survived {survivedDaysAmount} day";
            }
            else
            {
                daysUI.text = $"you have survived {survivedDaysAmount} days";
            }
        }
        else if(!canSleep)
        {
            // TODO: Add more conditions to this, for gambling and quota
            daysUI.text = $"Can't sleep just yet...";
        }

        daysUI.gameObject.SetActive(true);
        StartCoroutine(HideTextAfterSeconds(textShowupTime));
    }

    private IEnumerator HideTextAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        daysUI.gameObject.SetActive(false);
    }

    public void SetSleep(bool setSleep)
    {
        canSleep = setSleep;
    }
}
