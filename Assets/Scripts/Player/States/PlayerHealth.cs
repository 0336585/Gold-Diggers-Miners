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

        UpdateHearts((int)maxHealth);
    }

    public override void TakeDamage(CharacterStats _entityTakingDamage, CharacterStats _entityDoingDamage)
    {
        base.TakeDamage(_entityTakingDamage, _entityDoingDamage);

        UpdateHearts((int)currentHealth);

    }

    public override void TakeDamageWithInt(CharacterStats _entityTakingDamage, int _damage)
    {
        base.TakeDamageWithInt(_entityTakingDamage, _damage);

        UpdateHearts((int)currentHealth);
    }

    void UpdateHearts(int _heartsToShow)
    {
        int heartsToShow = _heartsToShow; // Default to showing 5 hearts

        // Clear previous hearts to avoid duplicates
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        for (int i = 0; i < heartsToShow; i++)
        {
            GameObject newHeart = null;

            // If maxHealth is greater than 5, replace the rightmost hearts with protected hearts
            if (maxHealth > 5 && i >= heartsToShow - (maxHealth - 5))
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
