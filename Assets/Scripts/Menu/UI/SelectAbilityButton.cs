using UnityEngine;
using UnityEngine.UI;
using MyCustomAttribute;

public class SelectAbilityButton : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField, ReadOnly] private AbsAbilityModule abilityModule;
    private Button button;
    private AbilityModuleManager abilityModuleManager;

    private void Awake() {
        abilityModuleManager = AbilityModuleManager.Instance;
        button = GetComponent<Button>();
    }

    private void Start() {
        abilityModule = abilityModuleManager.GetRandomAbility();
        text.text = abilityModule.abilityName;
    }
    
    private void OnEnable() {
        button.onClick.AddListener(SelectAbility);
    }

    private void OnDisable() {
        button.onClick.RemoveListener(SelectAbility);
    }

    private void SelectAbility() {
        abilityModuleManager.AddAbility(abilityModule);
    }
}
