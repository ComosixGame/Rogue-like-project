using UnityEngine;
using MyCustomAttribute;

public class IncreaseSpeed : AbsAbilityModule
{
    [SerializeField, Label("Value(%)")] private float value;
    private PlayerController controller;
    private float preSpeed;

    public override void AbilityActive()
    {
        controller = GetComponent<PlayerController>();
        preSpeed = controller.speed;
        controller.speed += preSpeed * (value / 100);
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        IncreaseSpeed increaseSpeed = parent.AddComponent<IncreaseSpeed>();
        increaseSpeed.tier = tier;
        increaseSpeed.abilityName = abilityName;
        increaseSpeed.description = description;
        increaseSpeed.value = value;
        increaseSpeed.enabled = true;
        return increaseSpeed;
    }

    public override void ResetAbility()
    {
        
    }
}
