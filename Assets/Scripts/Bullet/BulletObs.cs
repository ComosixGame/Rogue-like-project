using UnityEngine;

public class BulletObs : MonoBehaviour
{
    [SerializeField] private int _TakeDame;
    void Start()
    {
        Invoke("AutoDestroy", 5f);
    }

    private void OnTriggerEnter(Collider other) {
        AutoDestroy();
        if(other.gameObject.TryGetComponent(out IDamageble damageble)){
            damageble.TakeDamge(_TakeDame);
        }
    }

    private void OnDisable() {
        CancelInvoke("AutoDestroy");
    }
    private void AutoDestroy(){
        Destroy(gameObject);
    }
}
