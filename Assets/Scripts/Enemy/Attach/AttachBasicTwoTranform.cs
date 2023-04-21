using UnityEngine;

public class AttachBasicTwoTranform : AbsAttach
{
        private Animator _animator;

    private int _StandAiming;

    private ObjectPoolerManager objectPoolerManager;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _StandAiming = Animator.StringToHash("StandAiming");
        objectPoolerManager = ObjectPoolerManager.Instance;
    }

    public override void Attack(Transform shootPosition, Transform shootPositionOne)
    {
        GameObjectPool newBullet = objectPoolerManager.SpawnObject(bullet, shootPosition.transform.position, shootPosition.transform.rotation);
        newBullet.GetComponent<AbsBullet>().Fire(shootPosition.forward.normalized);
        GameObjectPool newBulletOne = objectPoolerManager.SpawnObject(bullet, shootPositionOne.transform.position, shootPositionOne.transform.rotation);
        newBulletOne.GetComponent<AbsBullet>().Fire(shootPositionOne.forward.normalized);
        OnAttackeComplete();
        _animator.SetBool(_StandAiming, false);
    }

    public override void Init()
    {
        
    }
}
