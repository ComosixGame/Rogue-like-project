using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyMission : MonoBehaviour
{
    public int index;
    public string nameDailyMission;
    public int coinReceive;

    [SerializeField] private Text _nameDailyMission;
    [SerializeField] private Text _coinReceive;
    [SerializeField] private GameObject _btnCompleteDailyMission;
    public static event Action<int> CompleteDailyMission;
    private GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
    }
    private void OnEnable() {
        //LoginGameDailyMission.OnLoginGame += LoginGame;
    }

    public void LoginGame(int indexMission){
        Debug.Log("login thanh cong");
    }

    private void Start() {
        _nameDailyMission.text = $"{nameDailyMission}";
        _coinReceive.text = $"{coinReceive}";   
    }

    public void HandleCompleteDailyMission(){
        Debug.Log(coinReceive);
        CompleteDailyMission?.Invoke(coinReceive);
    }

    private void OnDisable() {
        //LoginGameDailyMission.OnLoginGame -= LoginGame;
    }
}
