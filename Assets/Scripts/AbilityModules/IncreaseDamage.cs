using UnityEngine;
using MyCustomAttribute;

public class IncreaseDamage : AbsAbilityModule
{
    [SerializeField, Label("Value(%)")] float value;
    private AbsPlayerAttack playerAttack;
    private float preDamage;

    public override void AbilityActive()
    {
        playerAttack = GetComponent<PlayerController>().playerAttack;
        preDamage = playerAttack.damage;
        playerAttack.damage += preDamage * (value/100); 
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        IncreaseDamage increaseDamage = parent.AddComponent<IncreaseDamage>();
        increaseDamage.tier = tier;
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
