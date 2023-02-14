using UnityEngine;

public class StunShot : AbsAbilityModule
{
    [SerializeField] private float stunTime;
    public override void AbilityActive()
    {
        Bullet.OnHitEnemy += StunEnemy;
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        StunShot stunShot = parent.AddComponent<StunShot>();
        stunShot.tier = tier;
        stunShot.abilityName = abilityName;
        stunShot.description = description;
        stunShot.stunTime = stunTime;
        stunShot.enabled = true;
        return stunShot;
    }

    public override void ResetAbility()
    {
        Bullet.OnHitEnemy -= StunEnemy;
    }

    private void StunEnemy(GameObjectPool bullet, Vector3 hitPoint, Transform enemy, float damage) {
        if(enemy.TryGetComponent(out EnemyDamageble enemyDamageble)) {
            enemyDamageble.Stun(stunTime);
        }
    }
}
