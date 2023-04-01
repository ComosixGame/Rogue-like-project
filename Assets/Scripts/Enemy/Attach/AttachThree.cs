using UnityEngine;

public class AttachThree : AbsAttach
{
    //ban dan ra 3 huong
    [SerializeField] private int numberOfBullet;
    [Range(0,360), SerializeField] private float angel;
    private float angelPerShot;
    private Animator _animator;
    private int _StandAiming;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _StandAiming = Animator.StringToHash("StandAiming");
    }

    public override void Init()
    {
        angelPerShot = angel/numberOfBullet - 1;
    }
    public override void Attack(Transform shootPosition)
    {   
        int k = 1;
        for(int i = 0 ; i < numberOfBullet; i++){
            Vector3 dir = Quaternion.AngleAxis( i * k * angelPerShot, Vector3.up) * shootPosition.forward;
            GameObject newBullet = Instantiate(bullet, shootPosition.position, Quaternion.LookRotation(dir));
            newBullet.GetComponent<EnemyBulletsBasic>().Fire(dir);
            k *= -1;
            _animator.SetBool(_StandAiming, false);
        }
        OnAttackeComplete();
    }
}
