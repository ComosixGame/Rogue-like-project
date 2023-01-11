using System.Reflection.Emit;
using UnityEngine;
using MyCustomAttribute;

public class IncreaseHealth : AbsAbilityModule
{
    [SerializeField, Label("Value(%)")] private float value;

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        IncreaseHealth increaseHealth = parent.AddComponent<IncreaseHealth>();
        increaseHealth.tier = tier;
        increaseHealth.abilityName = abilityName;
        increaseHealth.description = description;
        increaseHealth.value = value;
        increaseHealth.enabled = true;
        return increaseHealth;
    }

    public override void AbilityActive()
    {
        Debug.Log($"tang {value}% mau");
    }

    public override void ResetAbility()
    {
        Debug.Log("reset mau");
    }
}
