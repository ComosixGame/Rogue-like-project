using System;
using UnityEngine;

public class SplitBullet : AbsAbilityModule
{
    private ObjectPoolerManager objectPoolerManager;
    
    protected override void Awake()
    {
        base.Awake();
        objectPoolerManager = ObjectPoolerManager.Instance;
    }

    public override void AbilityActive()
    {
        Bullet.OnHitEnemy += ActiveSplitBullet;
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        SplitBullet splitBullet = parent.AddComponent<SplitBullet>();
        splitBullet.tier = tier;
        splitBullet.abilityName = abilityName;
        splitBullet.description = description;
        splitBullet.enabled = true;
        return splitBullet;
    }

    public override void ResetAbility()
    {
        Bullet.OnHitEnemy -= ActiveSplitBullet;
    }
    
    private void ActiveSplitBullet(GameObjectPool bullet, Vector3 hitPoint, Transform splitTarget, float damage) {
        for(int i = 0; i < 4; i++) {
            Vector3 dir;
            switch(i) {
                case 0:
                    dir = Vector3.forward;
                    break;
                case 1:
                    dir = Vector3.left;
                    break;
                case 2:
                    dir = Vector3.right;
                    break;
                case 3:
                    dir = Vector3.back;
                    break;
                default:
                    throw new InvalidCastException();
            }
            GameObjectPool obj = objectPoolerManager.SpawnObject(bullet, hitPoint, Quaternion.LookRotation(dir));
            Bullet newBullet = obj.GetComponent<Bullet>();
            newBullet.splitBullet = true;
            newBullet.excludeTarget = splitTarget;
            newBullet.Fire(damage);
        }
    }

}
