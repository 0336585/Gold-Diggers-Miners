using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private string npcName = "Example Name";
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private float chanceToSpeakChance = 50f;
    [SerializeField] private float timeToShow = 2f;
    [SerializeField] private float maxDistance = 2f;
    [SerializeField] private GameObject dialogueTextPrefab;
    [SerializeField] private GameObject nameTextPrefab;

    private GameObject activeDialoguePrefab;
    private GameObject activeNamePrefab;
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
        if ((activeDialoguePrefab != null || activeNamePrefab != null) && playerTransform != null)
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position);
            if (distance > maxDistance)
            {
                Destroy(activeDialoguePrefab);
                Destroy(activeNamePrefab);
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
        if (dialogueLines.Length == 0 || dialogueTextPrefab == null || nameTextPrefab == null || canvasTransform == null)
            return;

        string randomLine = dialogueLines[Random.Range(0, dialogueLines.Length)];

        if (activeDialoguePrefab != null)
            Destroy(activeDialoguePrefab);
        if (activeNamePrefab != null)
            Destroy(activeNamePrefab);

        // Instantiate dialogue box in Canvas
        activeDialoguePrefab = Instantiate(dialogueTextPrefab, canvasTransform);
        activeNamePrefab = Instantiate(nameTextPrefab, canvasTransform);

        Debug.Log("NPC is speaking");

        // Get Text Components
        TextMeshProUGUI dialogueText = activeDialoguePrefab.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI nameText = activeNamePrefab.GetComponentInChildren<TextMeshProUGUI>();
        RectTransform dialogueRect = activeDialoguePrefab.GetComponent<RectTransform>();
        RectTransform nameRect = activeNamePrefab.GetComponent<RectTransform>();

        if (dialogueText != null && nameText != null)
        {
            dialogueText.text = randomLine;
            nameText.text = npcName;

            // Ensure Layout updates
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueRect);
            LayoutRebuilder.ForceRebuildLayoutImmediate(nameRect);

            // Resize background to be bigger than text
            Vector2 textSize = dialogueText.GetPreferredValues();
            dialogueRect.sizeDelta = new Vector2(textSize.x * 1.2f, textSize.y * 1.2f);

            Vector2 nameSize = nameText.GetPreferredValues();
            nameRect.sizeDelta = new Vector2(nameSize.x * 1.2f, nameSize.y * 1.2f);  

            // Positioning
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
            dialogueRect.position = screenPosition;

            // Position name box slightly above dialogue box
            nameRect.position = screenPosition + new Vector3(0, dialogueRect.sizeDelta.y * 1.2f, 0);
        }

        // Auto-destroy after timeToShow seconds
        Destroy(activeDialoguePrefab, timeToShow);
        Destroy(activeNamePrefab, timeToShow);
    }
}
