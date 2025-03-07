using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MugshotAnimator : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Health Sprites")]
    [SerializeField] private Sprite[] lowHealthSprites;
    [SerializeField] private Sprite[] mediumHealthSprites;
    [SerializeField] private Sprite[] highHealthSprites;

    [Header("Settings")]
    [SerializeField] private float minSwitchTime = 0.5f;
    [SerializeField] private float maxSwitchTime = 0.5f;
    [SerializeField] private float initialHoldTime = 0.2f;  // Time to hold the initial sprite for

    private void Start()
    {
        // Make sure the sprite starts from the correct health state (high health)
        UpdateSprite();

        // Start the random sprite swapping after an initial hold time
        StartCoroutine(StartWithHoldTime());
    }

    private IEnumerator StartWithHoldTime()
    {
        // Hold the initial sprite for a short period of time
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
            float switchInterval = Random.Range(minSwitchTime, maxSwitchTime);
            yield return new WaitForSeconds(switchInterval);
        }
    }

    private void UpdateSprite()
    {
        int currentHealth = playerHealth.GetCurrentHealth();
        Sprite[] selectedSpriteArray = null;

        // Determine which health category to use for the sprite
        if (currentHealth >= 3)   
        {
            selectedSpriteArray = highHealthSprites;
        }
        else if (currentHealth >= 2)   
        {
            selectedSpriteArray = mediumHealthSprites;
        }
        else if (currentHealth >= 1)   
        {
            selectedSpriteArray = lowHealthSprites;
        }
        else
        {
            selectedSpriteArray = lowHealthSprites; 
        }

        // Randomly pick a sprite from the selected array based on health
        int randomIndex = Random.Range(0, selectedSpriteArray.Length);

        // Update the sprite with the randomly chosen sprite from the selected array
        characterImage.sprite = selectedSpriteArray[randomIndex];
    }
}
