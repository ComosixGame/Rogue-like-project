using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject _level;

    public GameObject _gameSettings;
    public GameObject _resume;
    private LoadSceneManager loadSceneManager;

    private void Awake() {
        _level.SetActive(false);
        loadSceneManager = LoadSceneManager.Instance;
    }

    public void BtnSetting(){
        _gameSettings.SetActive(true);
    }

    public void BtnLobby(){
        _level.SetActive(false);
        _resume.SetActive(false);
    }

    public void BtnResume(){
        _resume.SetActive(false);
    }

    public void LoadScene(string path){
        loadSceneManager.LoadScene(path);
    }
}
