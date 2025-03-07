using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class SleepingBehaviour : MonoBehaviour
{
    private uint survivedDaysAmount;
    [SerializeField] GameObject keyPressPopUp;
    [SerializeField] TMP_Text daysUI;
    private bool inSleepRange = false;
    private void Start()
    {
        keyPressPopUp.SetActive(false);
    }
    private void Update()
    {
        if (inSleepRange)
            SleepableState();
    }
    void SleepableState()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Sleep();
        }
    }
    void Sleep()
    {
        survivedDaysAmount++;
        if(survivedDaysAmount == 1)
        {
            daysUI.text = $"you have survived {survivedDaysAmount} day";
        }
        else
        {
            daysUI.text = $"you have survived {survivedDaysAmount} days";
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BasePlayer>())
        {
            inSleepRange = true;
            keyPressPopUp.SetActive(true);
        }
    }    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BasePlayer>())
        {
            inSleepRange = false;
            keyPressPopUp.SetActive(false);
        }
    }
}
