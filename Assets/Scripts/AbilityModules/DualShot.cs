using UnityEngine;

public class DualShot : AbsAbilityModule
{
    [SerializeField] private Vector3 offset;
    private AbsPlayerAttack playerAttack;
    private ObjectPoolerManager objectPoolerManager;
#if UNITY_EDITOR
    private Vector3 shootPos;
#endif
    protected override void Awake()
    {
        base.Awake();
        objectPoolerManager = ObjectPoolerManager.Instance;
    }

    public override void AbilityActive()
    {
        playerAttack = GetComponent<PlayerController>().playerAttack;
        playerAttack.OnAttack += Shot;
    }

    public override AbsAbilityModule AddAbility(GameObject parent)
    {
        DualShot dualShot = parent.AddComponent<DualShot>();
        dualShot.tier = tier;
        dualShot.abilityName = abilityName;
        dualShot.description = description;
        dualShot.offset = offset;
        dualShot.enabled = true;
        return dualShot;
    }

    public override void ResetAbility()
    {
        if(playerAttack != null) {
            playerAttack.OnAttack += Shot;
        }
    }

    private void Shot(Transform attackPoint) {
        Vector3 newShootPos = attackPoint.position + offset;
        shootPos = newShootPos;
        playerAttack.Fire(newShootPos, attackPoint.rotation);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(shootPos, 1f);
    }
#endif
}
