using UnityEngine;

public abstract class AbsPlayerAttack : MonoBehaviour
{
    [SerializeField] protected GameObjectPool bullet;
    [SerializeField] protected ParticleSystem attackEffect;
    public float damage;
    protected ObjectPoolerManager objectPoolerManager;

    protected virtual void Awake() {
        objectPoolerManager = ObjectPoolerManager.Instance;
    }

    public abstract void Attack();

}
