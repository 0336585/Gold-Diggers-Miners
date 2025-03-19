using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : BaseHealth
{
    [Header("Player Hearts")]
    [SerializeField] private GameObject heartHolder;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject heartHalfPrefab;
    [SerializeField] private GameObject deathScreen;

    [Header("Post Processessing")]
    [SerializeField] private float damageEffectIntensity = 0.6f;
    [SerializeField] private float defaultEffectIntensity = 0.3f;
    [SerializeField] private float effectDuration = 0.2f;
    [SerializeField] private List<Volume> postProcessVolumes; // All Post Process Volumes in scene

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip takingDamageClip;

    private List<GameObject> hearts = new List<GameObject>();
    private Vignette activeVignette; // Reference to active vignette effect

    public override void Start()
    {
        base.Start();
        currentHealth = 10;
        UpdateHearts((int)currentHealth);
    }

    public override void TakeDamage(CharacterStats _entityTakingDamage, CharacterStats _entityDoingDamage)
    {
        base.TakeDamage(_entityTakingDamage, _entityDoingDamage);
        UpdateHearts((int)currentHealth);
        FlashVignette();
        audioSource.PlayOneShot(takingDamageClip);
        Die();
    }

    public override void TakeDamageWithInt(CharacterStats _entityTakingDamage, int _damage)
    {
        base.TakeDamageWithInt(_entityTakingDamage, _damage);
        UpdateHearts((int)currentHealth);
        FlashVignette();
        audioSource.PlayOneShot(takingDamageClip);
        Die();
    }

    private void Die()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            deathScreen.SetActive(true);
        }
    }

    public void UpdateHearts(int _currentHealth)
    {
        if (heartPrefab == null || heartHalfPrefab == null) return;

        int maxHearts = 10;  // UI now displays up to 10 hearts (20 HP max)

        // Clear previous hearts
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        // Total HP divided into full hearts and half-hearts
        int fullHearts = _currentHealth / 2;
        bool hasHalfHeart = _currentHealth % 2 != 0;

        for (int i = 0; i < maxHearts; i++)
        {
            GameObject newHeart = null;

            if (fullHearts > 0)
            {
                // Normal full hearts
                newHeart = Instantiate(heartPrefab, heartHolder.transform);
                fullHearts--;
            }
            else if (hasHalfHeart)
            {
                // Half-heart if remaining
                newHeart = Instantiate(heartHalfPrefab, heartHolder.transform);
                hasHalfHeart = false;
            }
            else
            {
                // No more hearts to display
                break;
            }

            hearts.Add(newHeart);
        }
    }

    public int GetCurrentHealth()
    {
        return (int)currentHealth;
    }

    public void SetMaxHealth()
    {
        currentHealth = maxHealth;
        UpdateHearts((int)currentHealth);
    }

    private void FlashVignette()
    {
        Volume activeVolume = GetActivePostProcessVolume();
        if (activeVolume != null && activeVolume.profile.TryGet(out activeVignette))
        {
            StartCoroutine(VignetteFlashCoroutine());
        }
    }

    private Volume GetActivePostProcessVolume()
    {
        foreach (Volume volume in postProcessVolumes)
        {
            if (volume.isActiveAndEnabled) // Check if volume is enabled
            {
                return volume;
            }
        }
        return null;
    }

    private IEnumerator VignetteFlashCoroutine()
    {
        if (activeVignette != null)
        {
            // Change to red and increase intensity
            activeVignette.color.Override(Color.red);
            activeVignette.intensity.Override(damageEffectIntensity);  

            yield return new WaitForSeconds(effectDuration);  

            // Return vignette color to black and reset intensity
            activeVignette.color.Override(Color.black);
            activeVignette.intensity.Override(defaultEffectIntensity);  
        }
    }
}
