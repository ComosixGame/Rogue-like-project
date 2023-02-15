using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldProtect : AbsAbilityModule
{
    [SerializeField] private Shield shield;
    private GameObject shieldClone;
    private ObjectPoolerManager objectPooler;

    protected override void Awake()
    {
        base.Awake();
        objectPooler = ObjectPoolerManager.Instance;
    }

    private void LateUpdate() {
        if(shieldClone != null) {
            shieldClone.transform.position = transform.position;
        }
    }

    public override void AbilityActive()
    {
        shieldClone = Instantiate(shield.gameObject, transform.position, transform.rotation);
    }
    

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        ShieldProtect shieldProtect = parent.AddComponent<ShieldProtect>();
        shieldProtect.tier = tier;
        shieldProtect.abilityName = abilityName;
        shieldProtect.description = description;
        shieldProtect.shield = shield;
        shieldProtect.enabled = true;
        return shieldProtect;
    }

    public override void ResetAbility()
    {
        Destroy(shieldClone);
    }
}
