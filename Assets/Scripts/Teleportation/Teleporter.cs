using TMPro;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    private bool inTeleportRange;

    [SerializeField] private GameObject keyPressPopUp;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject player;
    private void Update()
    {
        if (inTeleportRange)
            TeleportableState();
    }
    void TeleportableState()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Teleport();
        }
    }
    void Teleport()
    {
        player.transform.position = target.transform.position;
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
