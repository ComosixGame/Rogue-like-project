using UnityEngine;
using MyCustomAttribute;

public class ExplosiveBullet : AbsAbilityModule
{
    [SerializeField] private float radius;
    [SerializeField, Label("Damage rate(%)")] private float damageRate;
    [SerializeField] private LayerMask excludeLayer;
#if UNITY_EDITOR
    private Vector3 hitPoint;
#endif

    public override void AbilityActive()
    {
        Bullet.OnHitEnemy += Explosive;
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        ExplosiveBullet explosiveBullet = parent.AddComponent<ExplosiveBullet>();
        explosiveBullet.tier = tier;
        explosiveBullet.abilityName = abilityName;
        explosiveBullet.description = description;
        explosiveBullet.radius = radius;
        explosiveBullet.damageRate = damageRate;
        explosiveBullet.excludeLayer = excludeLayer;
        explosiveBullet.enabled = true;
        return explosiveBullet;
    }

    public override void ResetAbility()
    {
        Bullet.OnHitEnemy -= Explosive;
    }

    private void Explosive(GameObjectPool bullet, Vector3 hitPoint, Transform enemy, float damage) {
        Collider[] hitColliders = Physics.OverlapSphere(hitPoint, radius, ~excludeLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            if(hitCollider.TryGetComponent(out IDamageble damageble)) {
                float d = damage * (damageRate/100);
                Vector3 dir = hitCollider.transform.position - hitPoint;
                dir.y = 0;
                damageble.TakeDamge(d, dir);
                this.hitPoint = hitPoint;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPoint, radius);
    }
#endif
}
