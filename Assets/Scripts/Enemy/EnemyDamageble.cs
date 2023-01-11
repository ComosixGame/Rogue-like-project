using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


[RequireComponent(typeof(GameObjectPool))]
public class EnemyDamageble : MonoBehaviour, IDamageble
{
    [SerializeField] private int amountCoins;
    [SerializeField] private AbsItemObjectPool coin;
    [SerializeField] private GameObjectPool destroyEffect;
    [SerializeField] private float maxHealth;
    
    public Slider healthEnemy;

    private float health;
    private bool destroyed, knockBack;
    private Vector3 dirKnockBack;
    private MeshRenderer meshRenderer;
    private MaterialPropertyBlock  materialPropertyBlock;
    private GameObjectPool gameObjectPool;
    private GameManager gameManager;
    private ObjectPoolerManager objectPoolerManager;
    private NavMeshAgent agent;
    public static event Action<Vector3> OnEnemiesDestroy;
    
    private void Awake() {
        gameManager = GameManager.Instance;
        objectPoolerManager = ObjectPoolerManager.Instance;
        gameObjectPool = GetComponent<GameObjectPool>();
        meshRenderer = GetComponent<MeshRenderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetFloat("_alpha_threshold", 1);
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start() {
        health = maxHealth;
        healthEnemy.maxValue = maxHealth;
        healthEnemy.value = maxHealth;
        gameManager.AddEnemy(transform);
    }

    private void Update() {
        float alphaThreshold = materialPropertyBlock.GetFloat("_alpha_threshold");
        if(alphaThreshold > 0) {
            float newAlphaThreshold = Mathf.MoveTowards(alphaThreshold, 0, 1f * Time.deltaTime);
            materialPropertyBlock.SetFloat("_alpha_threshold", newAlphaThreshold);
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
        }

        if(knockBack) {
            agent.Move(dirKnockBack * 3f * Time.deltaTime);
        }
    }

    public void TakeDamge(float damage, Vector3 force)
    {
        dirKnockBack = force.normalized;
        CancelInvoke("CancelKnockBack");
        health -= damage;
        knockBack = true;
        Invoke("CancelKnockBack", 0.1f);
        healthEnemy.value = health;
        if(health <= 0 && !destroyed) {
            destroyed = true;
            Destroy();
        } 
    }

    private void CancelKnockBack() {
        knockBack = false;
    }

    public void Destroy() {
        objectPoolerManager.SpawnObject(destroyEffect, transform.position, Quaternion.identity);
        gameManager.RemoveEnemy(transform);
        //spawn coin
        for(int i = 0; i < amountCoins; i++) {
            objectPoolerManager.SpawnObject(coin, transform.position, Quaternion.identity);
        }
        objectPoolerManager.DeactiveObject(gameObjectPool);
        OnEnemiesDestroy?.Invoke(transform.position);
    }
}
