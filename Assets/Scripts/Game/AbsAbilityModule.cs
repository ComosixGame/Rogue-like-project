using UnityEngine;


public abstract class AbsAbilityModule : MonoBehaviour {
    public enum Tier {
        Common = 70,
        Rare = 20,
        Legendary = 10
    }
    public Tier tier;
    public Sprite image;
    public string abilityName;
    public string description;
    public abstract AbsAbilityModule AddAbility(GameObject parent);
    public abstract void AbilityActive();
    public abstract void ResetAbility();

    protected virtual void Awake() {
        this.enabled = false;
    }

    private void OnEnable() {
        AbilityActive();
    }
} 
