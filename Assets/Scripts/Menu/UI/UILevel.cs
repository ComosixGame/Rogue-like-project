using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevel : MonoBehaviour
{
    public GameObject _level;
    public GameObject _popup;
    public GameObject _popUpPause;
    public GameObject _popUpSetting;

    private void Awake() {
        _level.SetActive(true);
        _popUpPause.SetActive(false);
        _popUpSetting.SetActive(false);
    }
    public void BtnPause(){
        _popUpPause.SetActive(true);
    }

    public void BtnSetting(){
        _popUpSetting.SetActive(true);
    }

    public void ClosePanelSetting(){
        _popUpSetting.SetActive(false);
    }
}
