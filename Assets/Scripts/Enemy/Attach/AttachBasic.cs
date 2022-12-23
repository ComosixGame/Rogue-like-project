using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachBasic : AbsAttach
{
    public override void Attack( Transform shootPosition)
    {
        GameObject newBullet = Instantiate(bullet, shootPosition.transform.position, shootPosition.transform.rotation);
        newBullet.GetComponent<AbsBullet>().Fire(shootPosition.forward.normalized);
        OnAttackeComplete();
    }

    public override void Init()
    {
        
    }

}

