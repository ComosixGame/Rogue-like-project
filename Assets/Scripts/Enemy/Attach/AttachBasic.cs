using UnityEngine;

public class AttachBasic : AbsAttach
{

    private Animator _animator;

    private int _StandAiming;

    private ObjectPoolerManager objectPoolerManager;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _StandAiming = Animator.StringToHash("StandAiming");
        objectPoolerManager = ObjectPoolerManager.Instance;
    }

    public override void Attack(Transform shootPosition)
    {
        GameObjectPool newBullet = objectPoolerManager.SpawnObject(bullet, shootPosition.transform.position, shootPosition.transform.rotation);
        newBullet.GetComponent<AbsBullet>().Fire(shootPosition.forward.normalized);
        OnAttackeComplete();
        _animator.SetBool(_StandAiming, false);
    }

    public override void Init()
    {
        
    }

}

