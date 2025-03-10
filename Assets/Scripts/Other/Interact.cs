using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    [SerializeField] GameObject keyPressPopUp;
    [SerializeField] UnityEvent onInteract;

    private AudioSource audioPlayer;
    private bool inRange = false;

    private void Start()
    {
        keyPressPopUp.SetActive(false);
        audioPlayer = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            onInteract.Invoke();
            audioPlayer.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BasePlayer>())
        {
            inRange = true;
            keyPressPopUp.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BasePlayer>())
        {
            inRange = false;
            keyPressPopUp.SetActive(false);
        }
    }
}
