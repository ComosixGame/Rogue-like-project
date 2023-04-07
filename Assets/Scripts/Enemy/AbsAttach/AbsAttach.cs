using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsAttach : MonoBehaviour
{
    public GameObjectPool bullet;

    public abstract void Init();
    public abstract void Attack(Transform shootPosition);
    public event Action OnAttacked;

    protected void OnAttackeComplete(){
        OnAttacked?.Invoke();
    }

}
