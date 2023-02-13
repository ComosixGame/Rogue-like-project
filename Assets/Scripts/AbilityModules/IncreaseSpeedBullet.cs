using UnityEngine;
using MyCustomAttribute;

public class IncreaseSpeedBullet : AbsAbilityModule
{
    [SerializeField, Label("Value(%)")] private float value;
    private AbsPlayerAttack playerAttack;
    private float preSpeed;

    
    public override void AbilityActive()
    {
        playerAttack = GetComponent<PlayerController>().GetPlayerAttackComp();
        preSpeed = playerAttack.speed;
        playerAttack.speed += preSpeed * (value/100);
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        IncreaseSpeedBullet increaseSpeedBullet = parent.AddComponent<IncreaseSpeedBullet>();
        increaseSpeedBullet.tier = tier;
        increaseSpeedBullet.abilityName = abilityName;
        increaseSpeedBullet.description = description;
        increaseSpeedBullet.value = value;
        increaseSpeedBullet.enabled = true;
        return increaseSpeedBullet;
    }

    public override void ResetAbility()
    {
        throw new System.NotImplementedException();
    }


}
