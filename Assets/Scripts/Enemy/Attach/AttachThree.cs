using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachThree : AbsAttach
{
    //ban dan ra 3 huong
    [SerializeField] private int numberOfBullet;
    [Range(0,360), SerializeField] private float angel;
    private float angelPerShot;
    public override void Init()
    {
        angelPerShot = angel/numberOfBullet - 1;
    }
    public override void HandleAttack(Transform shootPosition)
    {   
        if(readyAttack == false){
            timerAttack += Time.deltaTime;
            if(timerAttack >= delayAttack){
                isFireBullet = true;
                timerAttack = 0;
                int k = 1;
                for(int i = 0 ; i < numberOfBullet; i++){
                    Vector3 dir = Quaternion.Euler(0, k * i * angelPerShot, 0) * shootPosition.forward;
                    GameObject newBullet = Instantiate(bullet, shootPosition.position, Quaternion.LookRotation(dir));
                    newBullet.GetComponent<EnemyBulletsBasic>().Fire(shootPosition.forward.normalized);
                    k *= -1;
                }
            }
        }
    }

}
