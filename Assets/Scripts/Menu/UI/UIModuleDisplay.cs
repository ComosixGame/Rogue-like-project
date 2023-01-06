using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModuleDisplay : MonoBehaviour
{
    [SerializeField] private Text nameModule;
    private AbilityModuleManager abilityModuleManager;

    private void Awake() {
        abilityModuleManager = AbilityModuleManager.Instance;
    }
//    private void OnEnable() {
//         abilityModuleManager.OnAddAbility += Module;
//    }

//    private void Module(List<AbsAbilityModule> abilityModulesActived){
//         foreach(AbsAbilityModule elem in abilityModulesActived){
//             nameModule.text = elem.abilityName;
//         }
//    }

    public void SetText(string name) {
        nameModule.text = name;
    }

//    private void OnDisable() {
//         abilityModuleManager.OnAddAbility -= Module;
//    }
}
