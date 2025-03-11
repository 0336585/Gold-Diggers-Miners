using TMPro;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject keyPressPopUp;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject player;
    [SerializeField] TownEntryChecker townEntryChecker;
    [SerializeField] private bool shouldSwitchPostProcess;
    private bool inTeleportRange;

    private void Update()
    {
        if (inTeleportRange)
            TeleportableState();
    }

    private void TeleportableState()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Teleport();
        }
    }
    private void Teleport()
    {
        player.transform.position = target.transform.position;

        if(shouldSwitchPostProcess)
            townEntryChecker.TogglePostProcessingSwitch();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BasePlayer>())
        {
            inTeleportRange = true;
            keyPressPopUp.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BasePlayer>())
        {
            inTeleportRange = false;
            keyPressPopUp.SetActive(false);
        }
    }
}
