using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MugshotAnimator : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Health Sprites")]
    [SerializeField] private Sprite[] lowHealthSprites;   // Array for low health (3 hearts or less)
    [SerializeField] private Sprite[] mediumHealthSprites; // Array for medium health (4-5 hearts)
    [SerializeField] private Sprite[] highHealthSprites;   // Array for high health (6-7 hearts)

    [Header("Settings")]
    [SerializeField] private float initialHoldTime = 0.2f;  // Time to hold the initial forward sprite

    private void Start()
    {
        // Start by updating the sprite immediately
        UpdateSprite();

        // Start the random sprite swapping after an initial hold time
        StartCoroutine(StartWithHoldTime());
    }

    private IEnumerator StartWithHoldTime()
    {
        // Hold the initial sprite for a few seconds
        yield return new WaitForSeconds(initialHoldTime);

        // Start the random sprite switching
        StartCoroutine(SwitchHealthSprite());
    }

    private IEnumerator SwitchHealthSprite()
    {
        while (true)
        {
            // Update health-based sprite if needed
            UpdateSprite();

            // Randomize the time interval between switching the sprite
            float switchInterval = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(switchInterval);
        }
    }

    private void UpdateSprite()
    {
        int currentHealth = playerHealth.GetCurrentHealth();
        Sprite[] selectedSpriteArray = null;

        // Determine which health category to use for the sprite
        if (currentHealth >= 6) // High health (6 or 7)
        {
            selectedSpriteArray = highHealthSprites;
        }
        else if (currentHealth >= 4) // Medium health (4 or 5)
        {
            selectedSpriteArray = mediumHealthSprites;
        }
        else if (currentHealth >= 1) // Low health (3 or fewer)
        {
            selectedSpriteArray = lowHealthSprites;
        }
        else
        {
            selectedSpriteArray = lowHealthSprites; // Handle case for 0 health (critical)
        }

        // Randomly pick a sprite from the selected array based on health
        int randomIndex = Random.Range(0, selectedSpriteArray.Length);

        // Update the sprite with the randomly chosen sprite from the selected array
        characterImage.sprite = selectedSpriteArray[randomIndex];

        Debug.Log(playerHealth.GetCurrentHealth());
    }
}
