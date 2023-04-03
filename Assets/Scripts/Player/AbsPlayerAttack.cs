using System;
using UnityEngine;
using MyCustomAttribute;

public abstract class AbsPlayerAttack : MonoBehaviour
{
    [SerializeField] protected GameObjectPool bullet;
    [SerializeField] protected ParticleSystem attackEffect;
    public float fireRateTime;
    public int MagazineCapacity;
    public float reloadDuration;
    public float damage;
    public float speed = 30f;
    [ReadOnly, SerializeField] private int remainingAmmo;
    [ReadOnly, SerializeField] private float reloadTime;
    private bool outOfAmmo;
    protected ObjectPoolerManager objectPoolerManager;
    //time reload bullet
    public static event Action<float> OnReloading;
    public event Action<Transform> OnAttack;
    private SoundManager soundManager;
    public virtual void Awake() {
        objectPoolerManager = ObjectPoolerManager.Instance;
        soundManager = SoundManager.Instance;
        remainingAmmo = MagazineCapacity;
    }

    private void Update() {
        if(outOfAmmo) {
            reloadTime += Time.deltaTime;
            reloadTime = reloadTime>reloadDuration ? reloadDuration : reloadTime;
            OnReloading?.Invoke(reloadTime);
            if(reloadTime >= reloadDuration) {
                remainingAmmo = MagazineCapacity;
                reloadTime = 0;
                outOfAmmo = false;
            }
        }
    }

    public bool Attack(){
        if(remainingAmmo > 0) {
            Fire(attackEffect.transform.position, attackEffect.transform.rotation);
            remainingAmmo--;
            OnAttack?.Invoke(attackEffect.transform);
            return true;
        } else {
            outOfAmmo = true;
            return false;
        }
    }

    public abstract void Fire(Vector3 shootPos, Quaternion shootRot);

}
