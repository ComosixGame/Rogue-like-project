using UnityEngine;

public class EnemyBulletsBasic : AbsBullet
{

    private float speedBullet = 10.0f;
    public override void Start()
    {
        Invoke("AutoDestroy", 5f);
    }

    public override void FixedUpdate()
    {
        if(fired) {
            _rb.velocity = transform.forward.normalized * speedBullet;
        }
    }

    public override void OnCollisionEnter(Collision other)
    {
        //throw new System.NotImplementedException();
    }
    public override void AutoDestroy()
    {
        Destroy(gameObject);
    }


    public override void Fire(Vector3 direction)
    {
        fired = true;
        dir = direction;
    }
}
