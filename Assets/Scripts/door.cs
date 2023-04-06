using UnityEngine;
using System;

public class door : MonoBehaviour
{
    public static event Action OnOpenDoor;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            OnOpenDoor?.Invoke();
        }
    }
        
}
