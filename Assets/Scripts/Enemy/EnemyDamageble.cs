using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageble : MonoBehaviour, IDamageble
{
    public ParticleSystem destroyEffect;
    [SerializeField] private float maxHealth;
    private float health;
    private bool destroyed;
    private GameManager gameManager;
    
    private void Awake() {
        gameManager = GameManager.Instance;
    }

    private void Start() {
        health = maxHealth;
        gameManager.enemies.Add(transform);
    }

    public void TakeDamge(float damage)
    {
       health -= damage;
       if(health <= 0 && !destroyed) {
            destroyed = true;
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy();
       } 
    }

    public void Destroy() {
        gameManager.enemies.Remove(transform);
        Destroy(gameObject);
    }
}
