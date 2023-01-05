using System;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(GameObjectPool))]
public class EnemyDamageble : MonoBehaviour, IDamageble
{
    [SerializeField] private GameObjectPool destroyEffect;
    [SerializeField] private float maxHealth;
    private float health;
    private bool destroyed, knockBack;
    private Vector3 dirKnockBack;
    private MeshRenderer meshRenderer;
    private MaterialPropertyBlock  materialPropertyBlock;
    private GameObjectPool gameObjectPool;
    private GameManager gameManager;
    private ObjectPoolerManager ObjectPoolerManager;
    private NavMeshAgent agent;
    public static event Action<Vector3> OnEnemiesDestroy;
    
    private void Awake() {
        gameManager = GameManager.Instance;
        ObjectPoolerManager = ObjectPoolerManager.Instance;
        gameObjectPool = GetComponent<GameObjectPool>();
        meshRenderer = GetComponent<MeshRenderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetFloat("_alpha_threshold", 1);
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start() {
        health = maxHealth;
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
        if(health <= 0 && !destroyed) {
                destroyed = true;
                Destroy();
        } 
    }

    private void CancelKnockBack() {
        knockBack = false;
    }

    public void Destroy() {
        ObjectPoolerManager.SpawnObject(destroyEffect, transform.position, Quaternion.identity);
        gameManager.RemoveEnemy(transform);
        ObjectPoolerManager.DeactiveObject(gameObjectPool);
        OnEnemiesDestroy?.Invoke(transform.position);
    }
}
