using System;
using UnityEngine;

public abstract class AbsBossAttack : MonoBehaviour
{
    public event Action OnAttackeComplete;
    public abstract void Attack();
    protected void AttackeComplete() {
        OnAttackeComplete?.Invoke();
    }
}
