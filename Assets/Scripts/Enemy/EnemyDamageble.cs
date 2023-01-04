using System;
using UnityEngine;


[RequireComponent(typeof(GameObjectPool))]
public class EnemyDamageble : MonoBehaviour, IDamageble
{
    [SerializeField] private GameObjectPool destroyEffect;
    [SerializeField] private float maxHealth;
    private float health;
    private bool destroyed;
    private MeshRenderer meshRenderer;
    private MaterialPropertyBlock  materialPropertyBlock;
    private GameObjectPool gameObjectPool;
    private GameManager gameManager;
    private ObjectPoolerManager ObjectPoolerManager;
    public static event Action<Vector3> OnEnemiesDestroy;
    
    private void Awake() {
        gameManager = GameManager.Instance;
        ObjectPoolerManager = ObjectPoolerManager.Instance;
        gameObjectPool = GetComponent<GameObjectPool>();
        meshRenderer = GetComponent<MeshRenderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetFloat("_alpha_threshold", 1);
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
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
    }

    public void TakeDamge(float damage)
    {
       health -= damage;
       if(health <= 0 && !destroyed) {
            destroyed = true;
            Destroy();
       } 
    }

    public void Destroy() {
        ObjectPoolerManager.SpawnObject(destroyEffect, transform.position, Quaternion.identity);
        gameManager.RemoveEnemy(transform);
        ObjectPoolerManager.DeactiveObject(gameObjectPool);
        OnEnemiesDestroy?.Invoke(transform.position);
    }
}
