using System.Collections.Generic;
using UnityEngine;
using MyCustomAttribute;

public class AbilityModuleSystem : MonoBehaviour
{
    [ReadOnly, SerializeField] private List<AbilityModule> listModule;

    public void AddAbility(AbilityModule abilityModule) {
        listModule.Add(abilityModule);
    }
}
