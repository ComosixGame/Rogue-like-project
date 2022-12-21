using UnityEngine;

public abstract class AbilityModule : MonoBehaviour {

    protected virtual void Start() {
        AbilityActive();
    }

    public abstract void AbilityActive();
} 
