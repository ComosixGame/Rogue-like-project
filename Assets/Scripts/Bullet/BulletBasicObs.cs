using System.Collections;
using System.Collections.Generic;
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
            damageble.TakeDamge(_TakeDame);
        }
    }

    private void OnDisable() {
        CancelInvoke("AutoDestroy");
    }

    public void AutoDestroy(){
        Destroy(gameObject);
    }
}
