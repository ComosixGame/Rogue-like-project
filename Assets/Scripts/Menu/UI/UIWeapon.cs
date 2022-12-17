using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWeapon : MonoBehaviour
{
   public GameObject _confirmWeapon;

   public void BtnWeapon(){
        _confirmWeapon.SetActive(false);
   }
}
