using UnityEngine;

public class BulletBasicObs : GameObjectPool
{
    [SerializeField] private int _TakeDame;
    private ObjectPoolerManager objectPoolerManager;

    private void Awake() {
        objectPoolerManager = ObjectPoolerManager.Instance;
    }

    public void Start()
    {
        Invoke("AutoDestroy", 5f);
    }
    public void OnCollisionEnter(Collision other)
    {
        AutoDestroy();
        if(other.gameObject.TryGetComponent(out IDamageble damageble)) {
            damageble.TakeDamge(_TakeDame, Vector3.zero);
        }
    }

    private void OnDisable() {
        CancelInvoke("AutoDestroy");
    }

    public void AutoDestroy(){
       objectPoolerManager.DeactiveObject(this);
    }
}
