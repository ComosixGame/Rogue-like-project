using UnityEngine;
using MyCustomAttribute;

public class PerfectCondition : AbsAbilityModule
{
    [SerializeField, Label("HP more than(%)")] private float minHealth;
    [SerializeField, Label("Value(%)")] private float value;
    private float preDamage;
    private PlayerDamageble playerDamageble;
    private AbsPlayerAttack playerAttack;
    private bool actived;

    public override void AbilityActive()
    {
        playerDamageble = GetComponent<PlayerDamageble>();
        playerAttack = GetComponent<PlayerController>().playerAttack;
        preDamage = playerAttack.damage;
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        PerfectCondition perfectCondition = parent.AddComponent<PerfectCondition>();
        perfectCondition.tier = tier;
        perfectCondition.abilityName = abilityName;
        perfectCondition.description = description;
        perfectCondition.minHealth = minHealth;
        perfectCondition.value = value;
        perfectCondition.enabled = true;
        return perfectCondition;
    }

    public override void ResetAbility()
    {
        
    }

    private void Update() {
        float healthPlayer = playerDamageble.health;
        if(healthPlayer >= playerDamageble.GetMaxHealth() * (minHealth/100)) {
            if(!actived) {
                actived = true;
                preDamage = playerAttack.damage;
                playerAttack.damage += preDamage * (value/100);
            }
        } else {
            actived = false;
            playerAttack.damage = preDamage;
        }
    }
    

}
