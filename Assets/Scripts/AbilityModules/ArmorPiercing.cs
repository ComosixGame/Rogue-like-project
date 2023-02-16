using UnityEngine;
using MyCustomAttribute;

public class ArmorPiercing : AbsAbilityModule
{
    [SerializeField, Label("Damage reduction rate(%)")] private float damageReductionRate;
    private AbsPlayerAttack playerAttack;
    private ObjectPoolerManager objectPoolerManager;

    protected override void Awake()
    {
        base.Awake();
        objectPoolerManager = ObjectPoolerManager.Instance;
    }

    public override void AbilityActive()
    {
        playerAttack = GetComponent<PlayerController>().playerAttack;
        Bullet.OnHitEnemy += Piercing;
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        ArmorPiercing armorPiercing = parent.AddComponent<ArmorPiercing>();
        armorPiercing.tier = tier;
        armorPiercing.abilityName = abilityName;
        armorPiercing.description = description;
        armorPiercing.damageReductionRate = damageReductionRate;
        armorPiercing.enabled = true;
        return armorPiercing;
    }

    public override void ResetAbility()
    {
        Bullet.OnHitEnemy -= Piercing;
    }

    private void Piercing(GameObjectPool bullet, Vector3 hitPoint, Transform target, Vector3 bulletDir,float damage) {
        float reduceDamage = damage - (damage * (damageReductionRate/100));
        GameObjectPool obj = objectPoolerManager.SpawnObject(bullet, hitPoint, Quaternion.LookRotation(bulletDir.normalized));
        Bullet newBullet = obj.GetComponent<Bullet>();
        newBullet.Fire(reduceDamage>=0?reduceDamage:0, playerAttack.speed);
    }
}
