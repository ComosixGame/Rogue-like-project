using UnityEngine;
using MyCustomAttribute;

public class IncreaseAmmo : AbsAbilityModule
{
    [SerializeField, Label("Value(%)")] private float value;
    private AbsPlayerAttack playerAttack;
    private int preAmmo;
    public override void AbilityActive()
    {
        playerAttack = GetComponent<PlayerController>().GetPlayerAttackComp();
        preAmmo = playerAttack.MagazineCapacity;
        playerAttack.MagazineCapacity += Mathf.RoundToInt(preAmmo * (value/100));

    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        IncreaseAmmo increaseAmmo = parent.AddComponent<IncreaseAmmo>();
        increaseAmmo.tier = tier;
        increaseAmmo.abilityName = abilityName;
        increaseAmmo.description = description;
        increaseAmmo.value = value;
        increaseAmmo.enabled = true;
        return increaseAmmo;
    }

    public override void ResetAbility()
    {
        
    }

}
