using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MugshotAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] forwardSprites;  // Array for forward health sprites (3 states for health)
    [SerializeField] private Sprite[] leftSprites;     // Array for left-facing health sprites
    [SerializeField] private Sprite[] rightSprites;    // Array for right-facing health sprites

    [Header("Settings")]
    [SerializeField] private float initialHoldTime = 2f;  // Time to hold the initial forward sprite

    private Image characterImage;
    private bool isLookingForward = true;
    private PlayerHealth playerHealth;
    private int currentHealthIndex = 0;

    private void OnEnable()
    {
        characterImage = GetComponent<Image>();
        characterImage.sprite = forwardSprites[0];  // Set initial full health sprite

        playerHealth = GetComponentInParent<PlayerHealth>();

        // Start with the player looking forward
        UpdateSprite();

        // Start the random head direction switching with an initial hold time
        StartCoroutine(StartWithHoldTime());
    }

    private IEnumerator StartWithHoldTime()
    {
        // Hold the initial forward-facing sprite for a few seconds
        yield return new WaitForSeconds(initialHoldTime);

        // Start the random head direction switching
        StartCoroutine(SwitchHeadDirection());
    }

    private IEnumerator SwitchHeadDirection()
    {
        while (true)
        {
            // Update health-based sprite if needed
            UpdateSprite();

            // Randomize time interval between switches
            float switchInterval = Random.Range(0.5f, 2f);
            yield return new WaitForSeconds(switchInterval);

            // Toggle head direction randomly
            isLookingForward = !isLookingForward;
            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        /*
        int currentHealth = playerHealth.GetCurrentHealth();

        // Set the sprite based on the current health
        if (currentHealth == 3)
        {
            currentHealthIndex = 0;  // Full health sprite
        }
        else if (currentHealth == 2)
        {
            currentHealthIndex = 1;  // Half health sprite
        }
        else
        {
            currentHealthIndex = 2;  // Low health sprite
        }

        // Update sprite based on current health index and direction
        if (isLookingForward)
        {
            characterImage.sprite = forwardSprites[currentHealthIndex];
        }
        else
        {
            // Randomize left or right facing sprites
            characterImage.sprite = Random.Range(0, 2) == 0 ? leftSprites[currentHealthIndex] : rightSprites[currentHealthIndex];
        }
        */
    }
}
