using UnityEngine;
using UnityEngine.UI;
using MyCustomAttribute;
using TMPro;

[RequireComponent(typeof(Button))]
public class SelectAbilityButton : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField, ReadOnly] private AbsAbilityModule abilityModule;
    private Button button;
    private AbilityModuleManager abilityModuleManager;
    private ObjectPoolerManager objectPoolerManager;

    private void Awake() {
        abilityModuleManager = AbilityModuleManager.Instance;
        objectPoolerManager = ObjectPoolerManager.Instance;
        button = GetComponent<Button>();
    }

    private void OnEnable() {
        button.onClick.AddListener(SelectAbility);
        abilityModuleManager.OnAddAbility += DestroyBtn;
    }

    private void OnDisable() {
        button.onClick.RemoveListener(SelectAbility);
        abilityModuleManager.OnAddAbility -= DestroyBtn;
    }

    public void SelectAbility() {
        abilityModuleManager.AddAbility(abilityModule);
    }

    public void SetAbilityModule(AbsAbilityModule abilityModule) {
        this.abilityModule = abilityModule;
        text.text = abilityModule.abilityName;
    }

    public void DestroyBtn(AbsAbilityModule newAbility) {
        Destroy(gameObject);
    }
}
