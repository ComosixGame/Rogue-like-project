using System.Reflection.Emit;
using UnityEngine;
using MyCustomAttribute;

public class IncreaseFireRate : AbsAbilityModule
{
    [SerializeField, Label("value(%)")] private float value;
    private PlayerController controller;
    private float preFireRate;
    public override void AbilityActive()
    {
        controller = GetComponent<PlayerController>();
        preFireRate = controller.fireRateTime;
        controller.fireRateTime -= preFireRate * (value/100); 
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        IncreaseFireRate increaseFireRate = parent.AddComponent<IncreaseFireRate>();
        increaseFireRate.tier = tier;
        increaseFireRate.abilityName = abilityName;
        increaseFireRate.description = description;
        increaseFireRate.value = value;
        return increaseFireRate;
    }

    public override void ResetAbility()
    {
        
    }

}
