using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public ParticleSystem impactEffect;
    private bool fired;

    void Start()
    {
        Invoke("AutoDestroy", 5f);
    }

    private void FixedUpdate() {
        if(fired) {
            rb.velocity = transform.forward.normalized * 30f;
        }
    }

    private void OnCollisionEnter(Collision other) {
        ContactPoint contact = other.GetContact(0);
        Instantiate(impactEffect, contact.point, Quaternion.LookRotation(contact.normal));
        AutoDestroy();
        if(other.gameObject.TryGetComponent(out IDamageble damageble)) {
            damageble.TakeDamge(1);
        }
    }

    public void Fire() {
        fired = true;
    }

    private void AutoDestroy() {
        Destroy(gameObject);
    }
}
