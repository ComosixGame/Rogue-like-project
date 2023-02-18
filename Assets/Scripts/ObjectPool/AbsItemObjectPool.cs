using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class AbsItemObjectPool : GameObjectPool
{
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private LayerMask layer;
    private Rigidbody rb;
    private ObjectPoolerManager objectPoolerManager;
    public bool readlyPickup;
    private SoundManager soundManager;

    protected virtual void Awake() {
        objectPoolerManager = ObjectPoolerManager.Instance;
        soundManager = SoundManager.Instance;
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable() {
        Vector3 dir = Random.insideUnitSphere.normalized;
        rb.AddForce(dir * 8f, ForceMode.Impulse);
        Invoke("ActivePickup", 1f);
    }

    private void OnTriggerEnter(Collider other) {
        if(!readlyPickup) return;
        if((layer & (1 << other.gameObject.layer)) != 0) {
            objectPoolerManager.DeactiveObject(this);
            ActiveItem(other);
            soundManager.PlaySound(pickupSound);
        }
    }

    private void OnDisable() {
        CancelInvoke("ActivePickup");
        readlyPickup = false;
    }

    private void ActivePickup() {
        readlyPickup = true;
    }

    public abstract void ActiveItem(Collider other);
}
