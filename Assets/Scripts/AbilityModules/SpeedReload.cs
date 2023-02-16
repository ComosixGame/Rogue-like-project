using UnityEngine;
using MyCustomAttribute;

public class SpeedReload : AbsAbilityModule
{
    [SerializeField, Label("Value(%)")] private float value;
    private AbsPlayerAttack playerAttack;
    private float preReloadTime;

    public override void AbilityActive()
    {
        playerAttack = GetComponent<PlayerController>().playerAttack;
        preReloadTime = playerAttack.reloadDuration;
        playerAttack.reloadDuration += preReloadTime * (value/100);
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        SpeedReload speedReload = parent.AddComponent<SpeedReload>();
        speedReload.tier = tier;
        speedReload.abilityName = abilityName;
        speedReload.description = description;
        speedReload.value = value;
        speedReload.enabled = true;
        return speedReload;
    }

    public override void ResetAbility()
    {
        throw new System.NotImplementedException();
    }
}
