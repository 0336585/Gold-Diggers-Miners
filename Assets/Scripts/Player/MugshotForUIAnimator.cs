using UnityEngine;
using System.Collections;

public class MugshotForUIAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer characterImage;

    [Header("Sprites")]
    [SerializeField] private Sprite[] mugshotSprites;

    [Header("Settings")]
    [SerializeField] private float minSwitchTime = 0.5f;
    [SerializeField] private float maxSwitchTime = 2f;
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
        // Randomly pick a sprite from the selected array based on health
        int randomIndex = Random.Range(0, mugshotSprites.Length);

        // Update the sprite with the randomly chosen sprite from the selected array
        characterImage.sprite = mugshotSprites[randomIndex];
    }
}
