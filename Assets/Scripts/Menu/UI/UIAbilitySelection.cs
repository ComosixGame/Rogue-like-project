using System.Collections.Generic;
using UnityEngine;

public class UIAbilitySelection : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    private AbilityModuleManager abilityModuleManager;

    private void Awake() {
        abilityModuleManager = AbilityModuleManager.Instance;
    }

    private void OnEnable() {
        abilityModuleManager.OnShowAbilityModuleSeletion += ShowUI;
        abilityModuleManager.OnAddAbility += HideUI;
    }

    private void OnDisable() {
        abilityModuleManager.OnShowAbilityModuleSeletion -= ShowUI;
        abilityModuleManager.OnAddAbility -= HideUI;
    }

    private void ShowUI(int numberAbilityAvaiable) {
        menu.SetActive(true);
    }

    private void HideUI(AbsAbilityModule newAbility ) {
        menu.SetActive(false);
    }
}
