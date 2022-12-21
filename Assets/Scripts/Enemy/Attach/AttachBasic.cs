using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachBasic : AbsAttach
{
    public override void HandleAttack( Transform shootPosition)
    {
        if(readyAttack == false){
            timerAttack += Time.deltaTime;
            if(timerAttack >= delayAttack){
                timerAttack = 0;
                isFireBullet = true;
                GameObject newBullet = Instantiate(bullet, shootPosition.transform.position, shootPosition.transform.rotation);
                newBullet.GetComponent<AbsBullet>().Fire(shootPosition.forward.normalized);
            }
        }
    }

    public override void Init()
    {
        
    }
}

