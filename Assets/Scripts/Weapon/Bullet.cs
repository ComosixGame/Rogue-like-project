using System;
using UnityEngine;
using MyCustomAttribute;
public class Bullet : MonoBehaviour
{
    public GameObjectPool impactEffect;
    [SerializeField] private Rigidbody rb;
    private bool fired, hit;
    [ReadOnly] public bool splitBullet;
    private float damage;
    private Collider colliderBullet;
    private GameObjectPool gameObjectPool;
    private ObjectPoolerManager ObjectPoolerManager;
    public static event Action<GameObjectPool, Vector3, float> OnHitEnemy;

    private void Awake() {
        ObjectPoolerManager = ObjectPoolerManager.Instance;
        gameObjectPool = GetComponent<GameObjectPool>();
        colliderBullet = GetComponent<Collider>();
    }

    private void FixedUpdate() {
        if(fired) {
            rb.velocity = transform.forward.normalized * 30f;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(!hit) {
            hit = true;
            Destroy();
            ContactPoint contact = other.GetContact(0);
            GameObjectPool effect = ObjectPoolerManager.SpawnObject(impactEffect , contact.point, Quaternion.LookRotation(contact.normal));
            if(other.gameObject.TryGetComponent(out IDamageble damageble)) {
                Vector3 dir = other.transform.position - transform.position;
                dir.y = 0;
                damageble.TakeDamge(damage, dir);
                if(!splitBullet) {
                    OnHitEnemy?.Invoke(gameObjectPool, contact.point, damage);
                }
            }
        }
    }

    public void Fire(float damage) {
        fired = true;
        this.damage = damage; 
    }

    private void Destroy() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        fired = false;
        hit = false;
        splitBullet = false;
        ObjectPoolerManager.DeactiveObject(gameObjectPool);
    }

    public void DeactiveCollision() {
        colliderBullet.isTrigger = true;
        Invoke("ActiveCollision", 0.2f);
    }
    
    public void ActiveCollision() {
        colliderBullet.isTrigger = false;
    }
}
