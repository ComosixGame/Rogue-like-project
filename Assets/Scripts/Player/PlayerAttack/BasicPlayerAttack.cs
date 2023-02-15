
using UnityEngine;

public class BasicPlayerAttack : AbsPlayerAttack
{
    public override void Fire(Vector3 shootPos, Quaternion shootRot)
    {
        attackEffect.Play();
        GameObjectPool newBullet = objectPoolerManager.SpawnObject(bullet, shootPos, shootRot);
        newBullet.GetComponent<Bullet>().Fire(damage, speed);
    }
}
