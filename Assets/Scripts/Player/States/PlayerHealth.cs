using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : BaseHealth
{
    [SerializeField] private Slider healthBar;

    public override void Start()
    {
        base.Start();

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public override void TakeDamage(CharacterStats _entityTakingDamage, CharacterStats _entityDoingDamage)
    {
        base.TakeDamage(_entityTakingDamage, _entityDoingDamage);

        healthBar.value = currentHealth;
    }

    public override void TakeDamageWithFloat(CharacterStats _entityTakingDamage, float _damage)
    {
        base.TakeDamageWithFloat(_entityTakingDamage, _damage);

        healthBar.value = currentHealth;
    }
}
