using UnityEngine;
using MyCustomAttribute;

public class IncreaseDamage : AbsAbilityModule
{
    [SerializeField, Label("Value(%)")] float value;

    public override void AbilityActive()
    {
        Debug.Log($"tăng {value}% damage");
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        IncreaseDamage increaseDamage = parent.AddComponent<IncreaseDamage>();
        increaseDamage.abilityName = abilityName;
        increaseDamage.description = description;
        increaseDamage.value = value;
        increaseDamage.enabled = true;
        return increaseDamage;
    }

    public override void ResetAbility()
    {
        
    }
}
