using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachVolleysOfBullets : AbsAttach
{
    [SerializeField] private int numberBullet;
    [SerializeField] private float timerVolleyOfBullets;
    
    public override void Init()
    {
        
    }

    public override void HandleAttack(Transform shootPosition)
    {
        if(readyAttack == false){
            timerAttack += Time.deltaTime;
            if(timerAttack >= delayAttack){
                if(numberBullet > 0){
                    timerAttack = 0;
                    isFireBullet = true;
                    GameObject newBullet = Instantiate(bullet, shootPosition.transform.position, shootPosition.transform.rotation);
                    newBullet.GetComponent<EnemyBulletsBasic>().Fire(shootPosition.forward.normalized);
                    --numberBullet;
                }
            }
            
            if(timerAttack >= timerVolleyOfBullets){
                timerAttack = 0;
                numberBullet = 3;
            }
        }
    }

}
