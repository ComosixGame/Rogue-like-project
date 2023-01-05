using UnityEngine;

public class HealingItem : AbsItemObjectPool
{
    [SerializeField] private float healthRestore;
    public override void ActiveItem(Collider other)
    {
        PlayerDamageble playerDamageble = other.GetComponent<PlayerDamageble>();
        playerDamageble.Heal(healthRestore);
    }
}
