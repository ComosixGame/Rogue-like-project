using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsBullet : MonoBehaviour
{
    public Rigidbody _rb;
    //public ParticleSystem impactEffect;
    public bool fired;

    public abstract void Start();

    public abstract void FixedUpdate();

    public abstract void OnCollisionEnter(Collision other);
    public abstract void Fire(Vector3 direction);

    public abstract void AutoDestroy();
}
