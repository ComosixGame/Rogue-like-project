using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public GameObject _chapter;
    public GameObject _level;

    public GameObject _gameSettings;
    public GameObject _lobby;
    public GameObject _resume;

    private LoadSceneManager loadSceneManager;

    private void Awake() {
        _chapter.SetActive(true);
        _level.SetActive(false);
        loadSceneManager = LoadSceneManager.Instance;
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

    public void LoadScene(string path){
        loadSceneManager.LoadScene(path);
    }
}
