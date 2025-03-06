using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : BaseHealth
{
    [SerializeField] private GameObject heartHolder;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject protectedHeartPrefab;

    private List<GameObject> hearts = new List<GameObject>();

    public override void Start()
    {
        base.Start();

        currentHealth = 3;
        UpdateHearts((int)currentHealth);
    }

    public override void TakeDamage(CharacterStats _entityTakingDamage, CharacterStats _entityDoingDamage)
    {
        base.TakeDamage(_entityTakingDamage, _entityDoingDamage);

        UpdateHearts((int)currentHealth);
        Die();
    }

    public override void TakeDamageWithInt(CharacterStats _entityTakingDamage, int _damage)
    {
        base.TakeDamageWithInt(_entityTakingDamage, _damage);

        UpdateHearts((int)currentHealth);
        Die();
    }

    private void Die()
    {
        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    public void UpdateHearts(int _currentHealth)
    {
        int heartsToShow = 5;  // Always show 5 hearts

        if (_currentHealth < 5)
            heartsToShow = _currentHealth;

        // Clear previous hearts to avoid duplicates
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        // Calculate how many protected hearts should be displayed based on the excess health
        int protectedHeartsCount = Mathf.Min((_currentHealth - 5), 5); // max of 3 protected hearts, if health exceeds 5

        // Display the hearts
        for (int i = 0; i < heartsToShow; i++)
        {
            GameObject newHeart = null;

            // If health exceeds 5, show protected hearts first (rightmost hearts are protected)
            if (i >= heartsToShow - protectedHeartsCount)
            {
                newHeart = Instantiate(protectedHeartPrefab, heartHolder.transform);
            }
            else
            {
                newHeart = Instantiate(heartPrefab, heartHolder.transform);
            }

            hearts.Add(newHeart);
        }
    }



}
