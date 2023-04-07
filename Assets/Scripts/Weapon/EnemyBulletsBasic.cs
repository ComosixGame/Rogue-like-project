using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBulletsBasic : AbsBullet
{
    //variable
    public float speed;
    [Tooltip("From 0% to 100%")]
    public float accuracy;
    public GameObject muzzlePrefab;

    private  Vector3 startPos;
    private Vector3 offset;
    private Rigidbody rb;
    private RotateToMouseScript rotateToMouse;
    private GameObject target;

    public override void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();

        if(muzzlePrefab != null){
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward + offset;
            var ps = muzzleVFX.GetComponent<ParticleSystem>();
            if(ps != null)
                Destroy(muzzleVFX, ps.main.duration);
            else{
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }
    }

    public override void FixedUpdate()
    {
        if(fired && speed != 0 && rb != null)
            rb.position += (transform.forward + offset) * (speed * Time.deltaTime);
    }

    private void OnDisable() {
        //CancelInvoke("AutoDestroy");
    }

    public override void AutoDestroy()
    {
        //Destroy(gameObject);
    }

    public void SetTarget(GameObject trg, RotateToMouseScript rotateTo){
        target = trg;
        rotateToMouse = rotateTo;
    }
    public override void Fire(Vector3 direction)
    {
        fired = true;
        dir = direction;
    }
}
