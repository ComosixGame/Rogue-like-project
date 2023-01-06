using UnityEngine;

public class PlayerDamageble : MonoBehaviour, IDamageble
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    // public ParticleSystem destroyEffect;
    private ObjectPoolerManager  objectPooler;
    private bool destroyed;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamge(float damage, Vector3 force){
        health -= damage;
        if(health <= 0 && !destroyed){
            destroyed = true;
            Destroy();
        }
    }

    public void Heal(float healthRestore) {
        if(!destroyed) {
            float h = health + healthRestore;
            health = h <= maxHealth ? h : maxHealth; 
        }
    }

    public void Destroy(){
        Destroy(gameObject);
    }
}
