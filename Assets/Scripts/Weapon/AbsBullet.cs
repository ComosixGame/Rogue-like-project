using UnityEngine;
using MyCustomAttribute;

public abstract class AbsBullet : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rb;
    //public ParticleSystem impactEffect;
    [SerializeField, ReadOnly] protected bool fired;
    protected Vector3 dir;

    public abstract void Start();

    public abstract void FixedUpdate();

    public abstract void OnCollisionEnter(Collision other);
    public abstract void Fire(Vector3 direction);

    public abstract void AutoDestroy();
}
