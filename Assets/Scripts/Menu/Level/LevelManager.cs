using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject _PopupPause;
    public GameObject _PopupReward;
    public GameObject _PopupSettings;
    public GameObject _PopupLobby;
    public GameObject _FooterLevel;
    public GameObject _Pause;
    public GameObject _Stage;
    public GameObject _LabUI;

    public void InitScene(){
        _PopupLobby.SetActive(false);
        _PopupPause.SetActive(false);
        _PopupReward.SetActive(false);
        _PopupSettings.SetActive(false);
        _Pause.SetActive(true);
        _Stage.SetActive(true);
        _LabUI.SetActive(true);
        _FooterLevel.SetActive(true);
    }

    public void HandlePause(){
        _FooterLevel.SetActive(false);
        _Stage.SetActive(false);
        _LabUI.SetActive(false);
        _Pause.SetActive(false);
        _PopupPause.SetActive(true);
    }

    public void HandleSettings(){
        _PopupSettings.SetActive(true);
    }

    public void HandleReturnLobby(){
        _PopupLobby.SetActive(true);
    }

    public void HandleResume(){
        InitScene();
    }

    public void HandleCloseSetting(){
        _PopupSettings.SetActive(false);
    }

    public void HandleClosePopupLobby(){
        _PopupLobby.SetActive(false);
    }
}
