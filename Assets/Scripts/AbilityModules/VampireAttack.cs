using UnityEngine;
using MyCustomAttribute;

public class VampireAttack : AbsAbilityModule
{
    [SerializeField, Label("Chance(%)")] private float chance;
    [SerializeField, Label("Value(%)")] private float value;
    private PlayerDamageble playerDamageble;

    public override void AbilityActive()
    {
        playerDamageble = GetComponent<PlayerDamageble>();
        EnemyDamageble.OnHit += Vampire;
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        VampireAttack vampireAttack = parent.AddComponent<VampireAttack>();
        vampireAttack.tier = tier;
        vampireAttack.abilityName = abilityName;
        vampireAttack.description = description;
        vampireAttack.chance = chance;
        vampireAttack.value = value;
        vampireAttack.enabled = true;
        return vampireAttack;
    }

    public override void ResetAbility()
    {
        EnemyDamageble.OnHit -= Vampire;
    }

    private void Vampire(Transform enemy, float damage) {
        int c = Random.Range(0, 100);
        if(c <= chance) {
            float heal = damage * (value/100);
            playerDamageble.Heal(heal);
        }
    }

}
