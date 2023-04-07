using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    [SerializeField] private GunScriptable weaponScriptable;
    private GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
    }
    
    private void Start() {
        // int index = gameManager.weaponSelected;
        // AbsPlayerAttack prefab = weaponScriptable.guns[index].prefabGun;
        // AbsPlayerAttack weapon = Instantiate(prefab, transform.position, Quaternion.identity);
        ///Debug.Log(prefab);
    }
}
