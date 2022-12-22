using UnityEngine;

[CreateAssetMenu(fileName = "New Ability Manager", menuName = "Scriptable Manager/Ability Manager")]
public class AbilityScripable : ScriptableObject
{
    public AbsAbilityModule[] abilityModules;
}
