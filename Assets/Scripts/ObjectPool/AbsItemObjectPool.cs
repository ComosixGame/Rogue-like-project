using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class AbsItemObjectPool : GameObjectPool
{
    [SerializeField] private LayerMask layer;
    private Rigidbody rb;
    private ObjectPoolerManager objectPoolerManager;
    protected virtual void Awake() {
        objectPoolerManager = ObjectPoolerManager.Instance;
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start() {
        Vector3 dir = Random.insideUnitSphere.normalized;
        rb.AddForce(dir * 8f, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other) {
        if((layer & (1 << other.gameObject.layer)) != 0) {
            objectPoolerManager.DeactiveObject(this);
            ActiveItem(other);
        }
    }

    public abstract void ActiveItem(Collider other);
}
