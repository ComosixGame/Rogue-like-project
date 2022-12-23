using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletRotation : AbsBullet
{
    public  float rotationSpeed;

    private float speedBullet = 10.0f;
    
    private Rigidbody rb;
    public override void Start()
    {
        Invoke("AutoDestroy", 5f);
        rb = GetComponent<Rigidbody>();    
    }

    public override void Fire(Vector3 direction)
    {
        fired = true;
        dir = direction;
    }

    public override void FixedUpdate()
    {
        rb.angularVelocity = Vector3.up * 1;
        if(fired){
            _rb.velocity = dir * speedBullet;
        }
    }

    public override void OnCollisionEnter(Collision other)
    {
        
    }
    public override void AutoDestroy()
    {
        Destroy(gameObject);
    }

}
