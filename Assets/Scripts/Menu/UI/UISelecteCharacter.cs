using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelecteCharacter : MonoBehaviour
{
    public GameObject _popupSelectCharacter;

    public void BtnSelectCharacter(){
        _popupSelectCharacter.SetActive(true);
        Debug.Log("okkk");
    }
}
