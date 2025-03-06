using TMPro;
using UnityEngine;

public class SleepingBehaviour : MonoBehaviour
{
    uint survivedDaysAmount;
    [SerializeField] TMP_Text DaysUI;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Sleep();
        }
    }

    public void Sleep()
    {
        survivedDaysAmount++;
        if(survivedDaysAmount == 1)
        {
            DaysUI.text = $"you have survived {survivedDaysAmount} days";
        }
        else
        {
            DaysUI.text = $"you have survived {survivedDaysAmount} days";
        }
    }
}
