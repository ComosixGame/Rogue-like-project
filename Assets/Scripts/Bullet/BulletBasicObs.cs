using UnityEngine;

public class BulletBasicObs : MonoBehaviour
{
    [SerializeField] private int _TakeDame;

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
        Destroy(gameObject);
    }
}
