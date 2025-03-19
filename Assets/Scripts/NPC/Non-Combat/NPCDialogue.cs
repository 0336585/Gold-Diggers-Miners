using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private float chanceToSpeakChance = 50f;
    [SerializeField] private float timeToShow = 2f;
    [SerializeField] private float maxDistance = 2f;
    [SerializeField] private GameObject dialogueTextPrefab;

    private GameObject activeDialoguePrefab;
    private Transform canvasTransform;
    private Transform playerTransform;

    private void Start()
    {
        canvasTransform = GameObject.Find("Canvas")?.transform;

        PlayerHealth playerHealth = Object.FindFirstObjectByType<PlayerHealth>();

        if (playerHealth != null)
        {
            playerTransform = playerHealth.transform;
        }
        else
        {
            Debug.LogError("PlayerHealth not found in the scene.");
        }

        if (canvasTransform == null)
        {
            Debug.LogError("Canvas not found! Make sure you have a Canvas in the scene.");
        }
    }

    private void Update()
    {
        if (activeDialoguePrefab != null && playerTransform != null)
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position);
            if (distance > maxDistance)
            {
                Destroy(activeDialoguePrefab);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            if (Random.Range(0f, 100f) <= chanceToSpeakChance)
            {
                ShowDialogue();
            }
        }
    }

    private void ShowDialogue()
    {
        if (dialogueLines.Length == 0 || dialogueTextPrefab == null || canvasTransform == null)
            return;

        string randomLine = dialogueLines[Random.Range(0, dialogueLines.Length)];

        if (activeDialoguePrefab != null)
            Destroy(activeDialoguePrefab);

        // Instantiate in Canvas
        activeDialoguePrefab = Instantiate(dialogueTextPrefab, canvasTransform);

        // Get Text and Background
        TextMeshProUGUI dialogueText = activeDialoguePrefab.GetComponentInChildren<TextMeshProUGUI>();
        RectTransform backgroundRect = activeDialoguePrefab.GetComponent<RectTransform>();

        if (dialogueText != null)
        {
            dialogueText.text = randomLine;

            // Ensure Layout updates
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundRect);

            // Resize background to be bigger than text
            Vector2 textSize = dialogueText.GetPreferredValues();
            backgroundRect.sizeDelta = new Vector2(textSize.x * 1.2f, textSize.y * 1.2f);
        }

        // Position UI text above the NPC using WorldToScreenPoint
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
        activeDialoguePrefab.GetComponent<RectTransform>().position = screenPosition;

        // Auto-destroy after timeToShow seconds (unless player moves away)
        Destroy(activeDialoguePrefab, timeToShow);
    }
}
