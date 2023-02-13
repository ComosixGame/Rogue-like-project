
public class BasicPlayerAttack : AbsPlayerAttack
{
    public override void Fire()
    {
        attackEffect.Play();
        GameObjectPool newBullet = objectPoolerManager.SpawnObject(bullet, attackEffect.transform.position, attackEffect.transform.rotation);
        newBullet.GetComponent<Bullet>().Fire(damage);
    }
}
