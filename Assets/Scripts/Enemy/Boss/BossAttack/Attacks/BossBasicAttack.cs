using UnityEngine;

public class BossBasicAttack : AbsBossAttack
{
    [SerializeField] private float damage, speed;
    [SerializeField] private ParticleSystem attackEffect;
    [SerializeField] private GameObjectPool bullet;
    [SerializeField] private Transform shootPoint;
    private ObjectPoolerManager objectPooler;
    
    private void Awake() {
        objectPooler = ObjectPoolerManager.Instance;
    }

    public override void Attack()
    {
        attackEffect.Play();
        GameObjectPool newBullet = objectPooler.SpawnObject(bullet, shootPoint.position, shootPoint.rotation);
        newBullet.GetComponent<Bullet>().Fire(damage, speed);
        AttackeComplete();
    }
}
