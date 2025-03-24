using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    [SerializeField] private GameObject keyPressPopUp;
    [SerializeField] private GameObject otherCanvas;
    [SerializeField] private bool canPlaySoundeffect = false;
    [SerializeField] private UnityEvent onInteract;

    private AudioSource audioPlayer;
    private bool inRange = false;

    private void Start()
    {
        keyPressPopUp.SetActive(false);
        audioPlayer = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (MenuManager.Instance.inMenu) return;


        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            onInteract.Invoke();
            if(canPlaySoundeffect)
                audioPlayer?.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BasePlayer>())
        {
            inRange = true;
            keyPressPopUp.SetActive(true);

            if (otherCanvas != null)
                otherCanvas.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BasePlayer>())
        {
            inRange = false;
            keyPressPopUp.SetActive(false);

            if (otherCanvas != null)
                otherCanvas.SetActive(false);
        }
    }
}
