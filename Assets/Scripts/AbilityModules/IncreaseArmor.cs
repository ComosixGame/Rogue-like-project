using System.Collections;
using UnityEngine;
using MyCustomAttribute;

public class IncreaseArmor : AbsAbilityModule
{
    [SerializeField, Label("Value(%)")] private float value;
    private float preArmor;
    private PlayerDamageble playerDamageble;

    public override void AbilityActive()
    {
        playerDamageble = GetComponent<PlayerDamageble>();
        preArmor = playerDamageble.armor;
        playerDamageble.armor += value;
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        IncreaseArmor increaseArmor = parent.AddComponent<IncreaseArmor>();
        increaseArmor.tier = tier;
        increaseArmor.abilityName = abilityName;
        increaseArmor.description = description;
        increaseArmor.value = value;
        increaseArmor.enabled = true;
        return increaseArmor;
    }

    public override void ResetAbility()
    {
        
    }
}
