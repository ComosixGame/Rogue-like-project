using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIModuleDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text nameModule;
    private AbilityModuleManager abilityModuleManager;

    private void Awake() {
        abilityModuleManager = AbilityModuleManager.Instance;
    }

    public void SetText(string name) {
        nameModule.text = name;
    }
}
