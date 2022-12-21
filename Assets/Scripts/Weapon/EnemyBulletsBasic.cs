using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletsBasic : AbsBullet
{
    public override void AutoDestroy()
    {
        Destroy(gameObject);
    }

    public override void Fire(Vector3 direction)
    {
        fired = true;
    }

    public override void FixedUpdate()
    {
        if(fired) {
            _rb.velocity = transform.forward.normalized * 50.0f;
        }
    }

    public override void OnCollisionEnter(Collision other)
    {
        throw new System.NotImplementedException();
    }

    public override void Start()
    {
        Invoke("AutoDestroy", 5f);
    }
}
