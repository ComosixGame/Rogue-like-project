using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Random=UnityEngine.Random;
using MyCustomAttribute;

public class DailyMissionManager : MonoBehaviour
{
    [SerializeField] DailyMissionScriptable dailyMissionScriptable;
    DailyMission currentDailyMission;
    [SerializeField] private DailyMission DailyMissionPrefab;
    [SerializeField, ReadOnly] List<DailyMission> dailyMissions = new List<DailyMission>();
    [SerializeField] private Transform DailyMissionParent;
    [SerializeField] private Scrollbar setPositionScrollbar;
    private List<int> mshowed = new List<int>();
    private GameManager gameManager;
    private float timePassed = 0;
    [SerializeField, Label("Time to recover daily mission(s)")] private float timeToRecoverDaily;
    [SerializeField, ReadOnly] private float timeLeft = 0;
    private DateTime dailyUpdateDateTime;

    private void Awake() {
        gameManager = GameManager.Instance;
        LoadData();
    }

    private void Start() {
        CountDownSecondTimeOnLoadGame();
        StartCoroutine(CountDownSecondTime());
    }

    private void OnApplicationQuit() {
        gameManager.SaveTimeDailyMission(dailyUpdateDateTime);
    }

    public void LoadData(){
        if(gameManager.firstTimeStart){
            RandomDailyMission();
        }
    }

    public void RandomDailyMission(){
        int loop = dailyMissionScriptable.dailyMissions.Length < 10 ? dailyMissionScriptable.dailyMissions.Length : 10;
        foreach(DailyMission mission in dailyMissions) {
            Destroy(mission.gameObject);
        }
        //reset danh sach
        mshowed.Clear();
        dailyMissions.Clear();

        for(int i = 0; i < loop; i++){
            int random = Random.Range(0, dailyMissionScriptable.dailyMissions.Length);
            while(mshowed.IndexOf(random) != -1) {
                random = Random.Range(0, dailyMissionScriptable.dailyMissions.Length);
            }
            mshowed.Add(random);
            DailyMission dailyMission = Instantiate(DailyMissionPrefab);
            dailyMissions.Add(dailyMission);
            dailyMission.transform.SetParent(DailyMissionParent, false);
            dailyMission.index = random;
            dailyMission.nameDailyMission = dailyMissionScriptable.dailyMissions[random].nameDailyMission;
            dailyMission.coinReceive = dailyMissionScriptable.dailyMissions[random].coinReceive;
            gameManager.dailyMissions.Add(random);
        }
        setPositionScrollbar.GetComponent<Scrollbar>().value = 1;
        gameManager.PlayerDataSave();
    }

    public void RenderDailyMission(){
        //reset danh sach
        mshowed.Clear();
        dailyMissions.Clear();

        foreach(int index in gameManager.dailyMissions) {
            DailyMission dailyMission = Instantiate(DailyMissionPrefab);
            dailyMission.transform.SetParent(DailyMissionParent, false);
            dailyMissions.Add(dailyMission);
            mshowed.Add(index);
            dailyMission.index = index;
            dailyMission.nameDailyMission = dailyMissionScriptable.dailyMissions[dailyMission.index].nameDailyMission;
            dailyMission.coinReceive = dailyMissionScriptable.dailyMissions[dailyMission.index].coinReceive;
        }
        setPositionScrollbar.GetComponent<Scrollbar>().value = 1;
        gameManager.PlayerDataSave();
    }

    private IEnumerator CountDownSecondTime() {
        while(true) {
            timePassed = (float)(DateTime.Now - dailyUpdateDateTime).TotalSeconds;
            //Debug.Log(timePassed);
            if(timePassed >= timeToRecoverDaily){
                gameManager.dailyMissions.Clear();
                RandomDailyMission();
                dailyUpdateDateTime = DateTime.Now;
            }
            yield return new WaitForSeconds(1f);
        }
    }


    public void CountDownSecondTimeOnLoadGame(){
        dailyUpdateDateTime = gameManager.dailyMissionDateTime;
        timePassed = (float)(DateTime.Now - dailyUpdateDateTime).TotalSeconds;

        if(timePassed >= timeToRecoverDaily){
            gameManager.dailyMissions.Clear();
            RandomDailyMission();
            dailyUpdateDateTime = DateTime.Now;
        } else {
            //data cu
            RenderDailyMission();
        }

    }
}
