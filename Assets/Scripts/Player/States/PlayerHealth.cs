using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : BaseHealth
{
    [SerializeField] private GameObject heartHolder;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject heartHalfPrefab;

    private List<GameObject> hearts = new List<GameObject>();

    public override void Start()
    {
        base.Start();

        currentHealth = 15;
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
}
