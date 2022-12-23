using System;
using System.Collections.Generic;
using UnityEngine;
using MyCustomAttribute;
using Random = UnityEngine.Random;

public class AbilityModuleManager : Singleton<AbilityModuleManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private AbilityScripable abilityScripable;
    [SerializeField, ReadOnly] private List<AbsAbilityModule> abilityModulesActived;
    [SerializeField] private List<AbsAbilityModule> ListAbilityShowed;
    private GameManager gameManager;
    public event Action OnShowAbilityModuleSeletion;
    public event Action OnAddAbility;

    
    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
        abilityModulesActived = new List<AbsAbilityModule>();
        ListAbilityShowed = new List<AbsAbilityModule>();
    }

    public AbsAbilityModule GetRandomAbility() {
        AbsAbilityModule[] abilityModules = abilityScripable.abilityModules;
        int randomIndex = Random.Range(0, abilityModules.Length);
        AbsAbilityModule ability = abilityModules[randomIndex];
        //random ko trùng lặp các ability đã show
        while(ListAbilityShowed.IndexOf(ability) != -1) {
            randomIndex = Random.Range(0, abilityModules.Length);
            ability = abilityModules[randomIndex];
        }
        ListAbilityShowed.Add(ability);
        return ability;
    }


    public void ShowAbilityModuleSeletion() {
        gameManager.PauseGame();
        OnShowAbilityModuleSeletion?.Invoke();
    }

    public void AddAbility(AbsAbilityModule abilityModule) {
        abilityModule.AddAbility(player);
        abilityModulesActived.Add(abilityModule);
        ListAbilityShowed.Clear();
        gameManager.ResumeGame();
        OnAddAbility?.Invoke();
    }

    public void ResetAbility() {
        foreach(AbsAbilityModule ability in abilityModulesActived) {
            ability.ResetAbility();
            Destroy(ability);
        }
        abilityModulesActived.Clear();
    }
    
}
