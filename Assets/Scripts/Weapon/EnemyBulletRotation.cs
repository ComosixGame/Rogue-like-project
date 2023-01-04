using UnityEngine;

public class EnemyBulletRotation : AbsBullet
{
    public  float rotationSpeed;

    private float speedBullet = 10.0f;

    //[SerializeField] private int _TakeDame;
    public override void Start()
    {
        //Invoke("AutoDestroy", 5f);
    }

    public override void Fire(Vector3 direction)
    {
        fired = true;
        dir = direction;
    }

    public override void FixedUpdate()
    {
        _rb.angularVelocity = Vector3.up * 1;
        if(fired){
            _rb.velocity = dir * speedBullet;
        }
    }

    private void OnDisable() {
        CancelInvoke("AutoDestroy");
    }
    public override void AutoDestroy()
    {
        //Destroy(gameObject);
    }


}
