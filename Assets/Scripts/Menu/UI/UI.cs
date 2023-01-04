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
    private void Awake() {
        _chapter.SetActive(true);
        _level.SetActive(false);
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

    // public void BtnPlayGame(){
    //     SceneManager.LoadScene("Scenes/Enemy");
    //     SceneManager.LoadScene("Scenes/Level", LoadSceneMode.Additive);
    // }
}
