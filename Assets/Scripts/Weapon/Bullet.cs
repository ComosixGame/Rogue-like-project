using System;
using UnityEngine;
using MyCustomAttribute;
public class Bullet : MonoBehaviour
{
    public GameObjectPool impactEffect;
    [SerializeField] private Rigidbody rb;
    private bool fired, hit;
    [ReadOnly] public bool splitBullet;
    [HideInInspector] public Transform excludeTarget;
    private float damage;
    private GameObjectPool gameObjectPool;
    private ObjectPoolerManager ObjectPoolerManager;
    public static event Action<GameObjectPool, Vector3, Transform, float> OnHitEnemy;

    private void Awake() {
        ObjectPoolerManager = ObjectPoolerManager.Instance;
        gameObjectPool = GetComponent<GameObjectPool>();
    }

    private void FixedUpdate() {
        if(fired) {
            rb.velocity = transform.forward.normalized * 30f;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform != excludeTarget && !hit) {
            hit = true;
            Destroy();
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            GameObjectPool effect = ObjectPoolerManager.SpawnObject(impactEffect , hitPoint, Quaternion.LookRotation(hitPoint - transform.position));
            if(other.gameObject.TryGetComponent(out IDamageble damageble)) {
                Vector3 dir = other.transform.position - transform.position;
                dir.y = 0;
                damageble.TakeDamge(damage, dir);
                if(!splitBullet) {
                    OnHitEnemy?.Invoke(gameObjectPool, hitPoint, other.transform, damage);
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
        excludeTarget = null;
        ObjectPoolerManager.DeactiveObject(gameObjectPool);
    }
}
