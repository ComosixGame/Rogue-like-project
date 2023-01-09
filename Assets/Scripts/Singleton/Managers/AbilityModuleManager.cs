using System;
using System.Linq;
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
    [SerializeField, ReadOnly] private List<AbsAbilityModule> absAbilityModulesCommon;
    [SerializeField, ReadOnly] private List<AbsAbilityModule> absAbilityModulesRare;
    [SerializeField, ReadOnly] private List<AbsAbilityModule> absAbilityModulesLegendary;
    private GameManager gameManager;
    public event Action OnShowAbilityModuleSeletion;
    public event Action<AbsAbilityModule> OnAddAbility;

    
    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
        abilityModulesActived = new List<AbsAbilityModule>();
        ListAbilityShowed = new List<AbsAbilityModule>();
        absAbilityModulesCommon = new List<AbsAbilityModule>();
        absAbilityModulesRare = new List<AbsAbilityModule>();
        absAbilityModulesLegendary = new List<AbsAbilityModule>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start() {
        foreach(AbsAbilityModule abilityModule in abilityScripable.abilityModules) {
            switch(abilityModule.tier) {
                case AbsAbilityModule.Tier.Common:
                    absAbilityModulesCommon.Add(abilityModule);
                    break;
                case AbsAbilityModule.Tier.Rare:
                    absAbilityModulesRare.Add(abilityModule);
                    break;
                case AbsAbilityModule.Tier.Legendary:
                    absAbilityModulesLegendary.Add(abilityModule);
                    break;
                default:
                    throw new InvalidCastException($"Tier {abilityModule.tier} does not exist");
            }
        }
    }

    public AbsAbilityModule GetRandomAbility() {
        AbsAbilityModule[] abilityModules;
        int randomIndex;
        int dropRateCommon = (int)AbsAbilityModule.Tier.Common;
        int dropRateRare = (int)AbsAbilityModule.Tier.Rare;
        // int dropRateLegendary = (int)AbsAbilityModule.Tier.Legendary;
        int dropChange = Random.Range(0, 100);
        if(dropChange < dropRateCommon) {
            randomIndex = Random.Range(0, absAbilityModulesCommon.Count);
            abilityModules = absAbilityModulesCommon.ToArray<AbsAbilityModule>();
        } else if (dropChange < dropRateCommon + dropRateRare) {
            randomIndex = Random.Range(0, absAbilityModulesRare.Count);
            abilityModules = absAbilityModulesRare.ToArray<AbsAbilityModule>();
        } else {
            randomIndex = Random.Range(0, absAbilityModulesLegendary.Count);
            abilityModules = absAbilityModulesLegendary.ToArray<AbsAbilityModule>();
        }
        
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
        Debug.Log(abilityModulesActived[0].abilityName);
        OnAddAbility?.Invoke(abilityModule);
    }

    public void ResetAbility() {
        foreach(AbsAbilityModule ability in abilityModulesActived) {
            ability.ResetAbility();
            Destroy(ability);
        }
        abilityModulesActived.Clear();
    }
    
}
