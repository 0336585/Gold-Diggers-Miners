using UnityEngine;

[RequireComponent(typeof(BaseHealth))]
public class CharacterStats : MonoBehaviour
{
    #region Stats
    [Header("Major stats")]
    [SerializeField] private Stat strength; // 1 point increase damage by 1 and crit.power by 1%
    [SerializeField] private Stat agility;  // 1 point increase evasion by 1% and crit.chance by 1%

    [Header("Offensive stats")]
    [SerializeField] private Stat damage;
    [SerializeField] private Stat critChance;
    [SerializeField] private Stat critPower;              // default value 150%

    [Header("Defensive stats")]
    [SerializeField] private Stat maxHealth;
    [SerializeField] private Stat armor;
    [SerializeField] private Stat evasion;
    #endregion

    #region Accessible stats
    [HideInInspector]
    public Stat Strength
    {
        get
        {
            return strength;
        }
    }
    [HideInInspector]
    public Stat Agility
    {
        get
        {
            return agility;
        }
    }

    [HideInInspector]
    public Stat Damage
    {
        get { return damage; }
    }
    [HideInInspector]
    public Stat CritChance
    {
        get { return critChance; }
    }
    [HideInInspector]
    public Stat CritPower
    {
        get { return critPower; }
    }

    [HideInInspector]
    public Stat Armor
    {
        get { return armor; }
    }
    [HideInInspector]
    public Stat Evasion
    {
        get { return evasion; }
    }
    #endregion

    private BaseHealth health;

    protected virtual void Awake()
    {
        health = GetComponent<BaseHealth>();

        critPower.SetDefaultValue(150);
        health.maxHealth = GetMaxHealthValue();
    }

    #region Stat calculations
    public int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    public bool CanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    public bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }


        return false;
    }

    public int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue();
    }

    #endregion
}
