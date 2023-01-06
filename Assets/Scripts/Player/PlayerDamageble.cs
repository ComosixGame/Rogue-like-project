using UnityEngine;

public class PlayerDamageble : MonoBehaviour, IDamageble
{
    [SerializeField] private float maxHealth;
    private float health;
    // public ParticleSystem destroyEffect;

    private ObjectPoolerManager  objectPooler;

    private bool destroyed;
    void Start()
    {
        health = maxHealth;
    }
    void Update()
    {
        
    }

    public void TakeDamge(float damage){
        health -= damage;
        if(health <= 0 && !destroyed){
            destroyed = true;
            Destroy();
        }
    }


    public void Destroy(){
        Destroy(gameObject);
    }
}
