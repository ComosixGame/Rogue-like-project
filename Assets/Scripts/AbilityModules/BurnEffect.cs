using UnityEngine;
using MyCustomAttribute;

public class BurnEffect : AbsAbilityModule
{   
    [SerializeField] private float time;
    [SerializeField, Label("Damage rate (%)")] float damageRate;
    public override void AbilityActive()
    {
        Bullet.OnHitEnemy += Burn;
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        BurnEffect burnEffect = parent.AddComponent<BurnEffect>();
        burnEffect.tier = tier;
        burnEffect.abilityName = abilityName;
        burnEffect.description = description;
        burnEffect.time = time;
        burnEffect.damageRate = damageRate;
        burnEffect.enabled = true;
        return burnEffect;
    }

    public override void ResetAbility()
    {
        Bullet.OnHitEnemy -= Burn;
    }

    private void Burn(GameObjectPool bullet, Vector3 hitPoint, Transform enemy, Vector3 bulletDir, float damage) {
        if(enemy.TryGetComponent(out EnemyDamageble enemyDamageble)) {
            float burnDamage = damage * (damageRate/100);
            enemyDamageble.Burn(burnDamage, time);
        }
    }
}
