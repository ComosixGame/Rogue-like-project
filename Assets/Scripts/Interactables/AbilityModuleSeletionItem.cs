using UnityEngine;

public class AbilityModuleSeletionItem : AbsItemObjectPool
{
    private AbilityModuleManager abilityModuleManager;

    protected override void Awake()
    {
        base.Awake();
        abilityModuleManager = AbilityModuleManager.Instance;
    }

    public override void ActiveItem(Collider other)
    {
        abilityModuleManager.ShowAbilityModuleSeletion();
    }
}
