using UnityEngine;
public class Bullet : MonoBehaviour
{
    public GameObjectPool impactEffect;
    [SerializeField] private Rigidbody rb;
    private bool fired;
    private GameObjectPool gameObjectPool;
    private ObjectPoolerManager ObjectPoolerManager;

    private void Awake() {
        ObjectPoolerManager = ObjectPoolerManager.Instance;
        gameObjectPool = GetComponent<GameObjectPool>();
    }

    private void FixedUpdate() {
        if(fired) {
            rb.velocity = transform.forward.normalized * 30f;
        }
    }

    private void OnCollisionEnter(Collision other) {
        ContactPoint contact = other.GetContact(0);
        GameObjectPool effect = ObjectPoolerManager.SpawnObject(impactEffect , contact.point, Quaternion.LookRotation(contact.normal));
        if(other.gameObject.TryGetComponent(out IDamageble damageble)) {
            damageble.TakeDamge(10);
        }
        Destroy();
    }

    private void OnDisable() {
        CancelInvoke("Destroy");
    }

    public void Fire() {
        Invoke("Destroy", 5f);
        fired = true;
    }

    private void Destroy() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        ObjectPoolerManager.DeactiveObject(gameObjectPool);
    }
}
