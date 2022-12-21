using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletRotation : AbsBullet
{
    private Vector3 dir;
    public override void AutoDestroy()
    {
        Destroy(gameObject);
    }

    public override void Fire(Vector3 direction)
    {
        fired = true;
        dir = direction;
    }

    public override void FixedUpdate()
    {
        if(fired){
            _rb.velocity = dir * 10.0f;
        }
    }

    public override void OnCollisionEnter(Collision other)
    {
        
    }

    public override void Start()
    {
        Invoke("AutoDestroy", 5f);
        
    }
}
