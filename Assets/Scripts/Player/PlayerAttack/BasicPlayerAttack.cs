
using UnityEngine;

public class BasicPlayerAttack : AbsPlayerAttack
{

    //singleton
    private SoundManager soundManager;

    //sound
    public AudioClip shootSound;
 
    public override void Awake() {
        base.Awake();
        soundManager = SoundManager.Instance;
    }

    protected override void Fire(Vector3 shootPos, Quaternion shootRot)
    {
        attackEffect.Play();
        GameObjectPool newBullet = objectPoolerManager.SpawnObject(bullet, shootPos, shootRot);
        newBullet.GetComponent<Bullet>().Fire(damage, speed);
        soundManager.PlaySound(shootSound);
    }
}
