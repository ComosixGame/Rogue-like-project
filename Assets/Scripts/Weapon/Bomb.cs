using System;
using UnityEngine;

public class Bomb : GameObjectPool
{
    [SerializeField] private LayerMask layerDamageble;
    [SerializeField] private LayerMask layerTrigger;
    public float radius;
    [SerializeField] private float damage;
    private ObjectPoolerManager objectPooler;
    public event Action OnExplosive;

    private void Awake() {
        objectPooler = ObjectPoolerManager.Instance;
    }

    private void OnCollisionEnter(Collision other) {
        if((layerTrigger & (1<<other.gameObject.layer)) != 0) {
            Explosive();
        }
    }

    private void Explosive() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, layerDamageble);
        foreach (Collider hitCollider in hitColliders)
        {
            if(hitCollider.TryGetComponent(out IDamageble damageble)) {
                Vector3 dir = hitCollider.transform.position - transform.position;
                dir.y = 0;
                damageble.TakeDamge(damage, dir);
            }
        }

        Destroy();
    }

    private void Destroy() {
        OnExplosive?.Invoke();
        objectPooler.DeactiveObject(this);
        OnExplosive = null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
