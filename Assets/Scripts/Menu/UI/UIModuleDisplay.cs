using UnityEngine;
using UnityEngine.UI;

public class UIModuleDisplay : MonoBehaviour
{
    [SerializeField] private Text nameModule;
    private AbilityModuleManager abilityModuleManager;

    private void Awake() {
        abilityModuleManager = AbilityModuleManager.Instance;
    }

    public void SetText(string name) {
        nameModule.text = name;
    }
}
