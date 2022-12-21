using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsAttach : MonoBehaviour
{
    public float delayAttack = 3.0f, timerAttack;
    public bool readyAttack, isFireBullet;

    public GameObject bullet;

    public abstract void Init();
    public abstract void HandleAttack(Transform shootPosition);
}
