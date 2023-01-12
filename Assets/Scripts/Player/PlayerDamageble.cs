using UnityEngine;
using MyCustomAttribute;
using UnityEngine.UI;

public class PlayerDamageble : MonoBehaviour, IDamageble
{
    [SerializeField] private float maxHealth;
    [ReadOnly, SerializeField] private float health;
    // public ParticleSystem destroyEffect;
    private ObjectPoolerManager objectPooler;
    private GameManager gameManager;

    public Slider healthPlayer;
    private bool destroyed;
    private Camera _camera;
    void Start()
    {
        health = maxHealth;
        healthPlayer.maxValue = maxHealth;
        healthPlayer.value   = maxHealth;
        _camera = Camera.main;
    }

    private void LateUpdate() {
        healthPlayer.transform.LookAt(_camera.transform);
    }


    public void TakeDamge(float damage, Vector3 force){
        health -= damage;
        healthPlayer.value = health;
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
