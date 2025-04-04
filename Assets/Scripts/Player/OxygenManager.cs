using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float oxygenLoseRate = 1f;
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float damageRate = 3f;

    private float currentOxygen;
    private PlayerHealth playerHealth;
    private Coroutine damageCoroutine;

    [SerializeField] private List<GameObject> oxygenBubbles;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip oxygenDepleteClip;

    private int lastOxygenLevel;  

    private void Start()
    {
        currentOxygen = maxOxygen;
        playerHealth = GetComponent<PlayerHealth>();
        lastOxygenLevel = Mathf.CeilToInt(currentOxygen / 10f);
        UpdateOxygenUI();
    }

    private void Update()
    {
        float currentYPosition = transform.position.y;

        if (currentYPosition < 0) // If player is underwater (negative Y)
        {
            currentOxygen -= oxygenLoseRate * Time.deltaTime;
            currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);

            // Check if oxygen level dropped by 10
            int currentOxygenLevel = Mathf.CeilToInt(currentOxygen / 10f);
            if (currentOxygenLevel < lastOxygenLevel)
            {
                PlayOxygenDepleteSound();
            }
            lastOxygenLevel = currentOxygenLevel;

            UpdateOxygenUI();
        }

        if (currentOxygen <= 0)
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DealDamageOverTime());
            }
        }
        else
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DealDamageOverTime()
    {
        CharacterStats playerStats = GetComponent<CharacterStats>();

        while (currentOxygen <= 0)
        {
            playerHealth.TakeDamageWithInt(playerStats, damageAmount);
            yield return new WaitForSeconds(damageRate);
        }
    }

    private void UpdateOxygenUI()
    {
        int activeBubbles = Mathf.CeilToInt(currentOxygen / 10f); // Determine how many bubbles should be active

        for (int i = 0; i < oxygenBubbles.Count; i++)
        {
            oxygenBubbles[i].SetActive(i < activeBubbles); // Enable only necessary bubbles
        }
    }

    public void AddMaxOxygen()
    {
        currentOxygen = maxOxygen;
        UpdateOxygenUI();
    }

    private void PlayOxygenDepleteSound()
    {
        audioSource.PlayOneShot(oxygenDepleteClip);
    }
}
