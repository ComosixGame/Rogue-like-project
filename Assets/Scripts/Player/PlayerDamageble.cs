using UnityEngine;
using MyCustomAttribute;
using UnityEngine.UI;
using System;

public class PlayerDamageble : MonoBehaviour, IDamageble
{
    public AudioClip hitSound;
    public AudioClip deadSound;
    [SerializeField] private float maxHealth;
    [Label("Armor(%)")] public float armor;
    public float health {get; private set;}
    private ObjectPoolerManager objectPooler;
    private GameManager gameManager;
    public Slider healthPlayer;
    private bool destroyed;
    private Camera _camera;
    private SoundManager soundManager;
    public static event Action OnLoseGame;


    void Start()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
        health = maxHealth;
        healthPlayer.maxValue = maxHealth;
        healthPlayer.value   = maxHealth;
        _camera = Camera.main;
    }

    private void LateUpdate() {
        healthPlayer.transform.LookAt(_camera.transform);
    }


    public void TakeDamge(float damage, Vector3 force){
        //soundManager.PlaySound(hitSound);
        damage -= damage * (armor/100);
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
            healthPlayer.value = health;
        }
    }

    public void Destroy(){
        //soundManager.PlaySound(deadSound);    
        gameManager.EndGame(false);
        gameObject.SetActive(false);
        OnLoseGame?.Invoke();
    }

    public float GetMaxHealth() {
        return maxHealth;
    }
}
