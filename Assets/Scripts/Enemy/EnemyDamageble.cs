using UnityEngine;

public class EnemyDamageble : MonoBehaviour, IDamageble
{
    [SerializeField] private string key;
    public ParticleSystem destroyEffect;
    [SerializeField] private float maxHealth;
    private float health;
    private bool destroyed;
    private MeshRenderer meshRenderer;
    private MaterialPropertyBlock  materialPropertyBlock;
    private GameManager gameManager;
    private ObjectPooler objectPooler;
    
    private void Awake() {
        gameManager = GameManager.Instance;
        objectPooler = ObjectPooler.Instance;
        meshRenderer = GetComponent<MeshRenderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetFloat("_alpha_threshold", 1);
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }

    private void Start() {
        health = maxHealth;
        gameManager.enemies.Add(transform);
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
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        gameManager.enemies.Remove(transform);
        objectPooler.InactiveObject(key, gameObject);
    }
}
