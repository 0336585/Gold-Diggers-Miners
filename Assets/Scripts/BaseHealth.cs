using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class BaseHealth : MonoBehaviour
{
    [field: SerializeField] public float maxHealth { get; set; }
    [field: SerializeField] public float currentHealth { get; set; }

    public virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(CharacterStats _entityTakingDamage, CharacterStats _entityDoingDamage)
    {
        if (_entityTakingDamage.CanAvoidAttack(_entityTakingDamage))
            return;

        int totalDamage = _entityDoingDamage.Damage.GetValue() + _entityDoingDamage.Strength.GetValue();

        if (_entityDoingDamage.CanCrit())
        {
            totalDamage = _entityDoingDamage.CalculateCriticalDamage(totalDamage);
        }

        totalDamage = _entityTakingDamage.CheckTargetArmor(_entityTakingDamage, totalDamage);

        currentHealth -= totalDamage;
    }
}
