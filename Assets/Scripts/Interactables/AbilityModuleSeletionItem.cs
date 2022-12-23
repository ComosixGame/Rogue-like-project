using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AbilityModuleSeletionItem : MonoBehaviour
{
    private AbilityModuleManager abilityModuleManager;

    private void Awake() {
        abilityModuleManager = AbilityModuleManager.Instance;
    }

    private void OnTriggerEnter(Collider other) {
        abilityModuleManager.ShowAbilityModuleSeletion();
        Destroy(gameObject);        
    }
    
}
