using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject _chapter;
    public GameObject _level;

    public GameObject _gameSettings;
    public GameObject _lobby;
    public GameObject _resume;
    private void Awake() {
        _chapter.SetActive(true);
        _level.SetActive(false);
    }
    public void BtnPlay(){
        _chapter.SetActive(false);
        _level.SetActive(true);
    }

    public void BtnSetting(){
        _gameSettings.SetActive(true);
    }

    public void BtnLobby(){
        _lobby.SetActive(true);
        _level.SetActive(false);
        _resume.SetActive(false);
    }

    public void BtnResume(){
        _resume.SetActive(false);
    }


}
