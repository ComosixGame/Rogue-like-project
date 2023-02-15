using System;
using UnityEngine;
using MyCustomAttribute;
public class Bullet : MonoBehaviour
{
    private float speed;
    public GameObjectPool impactEffect;
    [SerializeField] private Rigidbody rb;
    private bool fired, hit;
    [ReadOnly] public bool splitBullet;
    [HideInInspector] public Transform excludeTarget;
    private float damage;
    private GameObjectPool gameObjectPool;
    private ObjectPoolerManager ObjectPoolerManager;
    public static event Action<GameObjectPool, Vector3, Transform, Vector3, float> OnHitEnemy;

    private void Awake() {
        ObjectPoolerManager = ObjectPoolerManager.Instance;
        gameObjectPool = GetComponent<GameObjectPool>();
    }

    private void FixedUpdate() {
        if(fired) {
            rb.velocity = transform.forward.normalized * speed;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform != excludeTarget && !hit) {
            hit = true;
            Vector3 hitPoint = other.GetComponent<Collider>().ClosestPoint(transform.position);
            GameObjectPool effect = ObjectPoolerManager.SpawnObject(impactEffect , hitPoint, Quaternion.identity);
            if(other.gameObject.TryGetComponent(out IDamageble damageble)) {
                Vector3 dir = other.transform.position - transform.position;
                dir.y = 0;
                damageble.TakeDamge(damage, dir);
                if(!splitBullet) {
                    OnHitEnemy?.Invoke(gameObjectPool, hitPoint, other.transform, transform.forward, damage);
                }
            }
            Destroy();
        }
    }

    public void Fire(float damage, float speed) {
        fired = true;
        this.damage = damage; 
        this.speed = speed;
    }

    private void Destroy() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        fired = false;
        hit = false;
        splitBullet = false;
        excludeTarget = null;
        ObjectPoolerManager.DeactiveObject(gameObjectPool);
    }
}
