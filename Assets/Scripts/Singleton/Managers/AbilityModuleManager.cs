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
    [SerializeField] private List<AbsAbilityModule> listAbilityShowed;
    [SerializeField, ReadOnly] private List<AbsAbilityModule> listAbilityAvailable;
    [SerializeField, ReadOnly] private List<AbsAbilityModule> absAbilityModulesCommon;
    [SerializeField, ReadOnly] private List<AbsAbilityModule> absAbilityModulesRare;
    [SerializeField, ReadOnly] private List<AbsAbilityModule> absAbilityModulesLegendary;
    private GameManager gameManager;
    private ObjectPoolerManager ObjectPoolerManager;
    public event Action<int> OnShowAbilityModuleSeletion;
    public event Action<AbsAbilityModule> OnAddAbility;

    
    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
        ObjectPoolerManager = ObjectPoolerManager.Instance;
        abilityModulesActived = new List<AbsAbilityModule>();
        listAbilityShowed = new List<AbsAbilityModule>();
        listAbilityAvailable = new List<AbsAbilityModule>();
        absAbilityModulesCommon = new List<AbsAbilityModule>();
        absAbilityModulesRare = new List<AbsAbilityModule>();
        absAbilityModulesLegendary = new List<AbsAbilityModule>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start() {
        listAbilityAvailable = abilityScripable.abilityModules.ToList<AbsAbilityModule>();
        AddAbiltyTier();
    }

    private void AddAbiltyTier() {
        foreach(AbsAbilityModule abilityModule in listAbilityAvailable) {
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

    private AbsAbilityModule GetRandomAbility() {
        AbsAbilityModule[] abilityModules = RandomDropRateAbility();
        int randomIndex = Random.Range(0, abilityModules.Length);
        AbsAbilityModule ability = abilityModules[randomIndex];
        //random ko trùng lặp các ability đã show
        while(listAbilityShowed.IndexOf(ability) != -1) {
            abilityModules = RandomDropRateAbility();
            randomIndex = Random.Range(0, abilityModules.Length);
            ability = abilityModules[randomIndex];
        }
        listAbilityShowed.Add(ability);
        return ability;
    }

    private AbsAbilityModule[] RandomDropRateAbility() {
        AbsAbilityModule[] abilityModules;
        int dropRateCommon = (int)AbsAbilityModule.Tier.Common;
        int dropRateRare = (int)AbsAbilityModule.Tier.Rare;
        // int dropRateLegendary = (int)AbsAbilityModule.Tier.Legendary;
        int dropChange = Random.Range(0, 100);
        if(dropChange < dropRateCommon) {
            abilityModules = absAbilityModulesCommon.ToArray<AbsAbilityModule>();
        } else if (dropChange < dropRateCommon + dropRateRare) {
            abilityModules = absAbilityModulesRare.ToArray<AbsAbilityModule>();
        } else {
            abilityModules = absAbilityModulesLegendary.ToArray<AbsAbilityModule>();
        }

        if(abilityModules.Length == 0) {
            abilityModules = RandomDropRateAbility();
        }

        return abilityModules;
    }

    public void RenderAbilitySelector(Transform container, SelectAbilityButton button, int size) {
        int s = size <= listAbilityAvailable.Count ? size : listAbilityAvailable.Count;
        for(int i = 0; i < s; i++) {
            SelectAbilityButton btn = ObjectPoolerManager.SpawnObject(button).GetComponent<SelectAbilityButton>();
            btn.transform.SetParent(container, false);
            btn.SetAbilityModule(GetRandomAbility());
        }
    }


    public void ShowAbilityModuleSeletion() {
        gameManager.PauseGame();
        OnShowAbilityModuleSeletion?.Invoke(listAbilityAvailable.Count);
    }

    public void AddAbility(AbsAbilityModule abilityModule) {
        abilityModule.AddAbility(player);
        abilityModulesActived.Add(abilityModule);
        listAbilityShowed.Clear();
        gameManager.ResumeGame();
        Debug.Log(abilityModulesActived[0].abilityName);
        OnAddAbility?.Invoke(abilityModule);
        //cập nhật lai danh sách ability mới sau
        absAbilityModulesCommon.Clear();
        absAbilityModulesRare.Clear();
        absAbilityModulesLegendary.Clear();
        listAbilityAvailable.Remove(abilityModule);
        AddAbiltyTier();
    }

    public void ResetAbility() {
        foreach(AbsAbilityModule ability in abilityModulesActived) {
            ability.ResetAbility();
            Destroy(ability);
        }
        abilityModulesActived.Clear();
    }
    
}
