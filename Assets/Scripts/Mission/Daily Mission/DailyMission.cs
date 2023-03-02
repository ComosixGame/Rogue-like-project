using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyMission : MonoBehaviour
{
    public int index;
    public bool completed;
    public string title;
    public string description;
    public int experienceReward;
    public int goldReward;
    public bool isReceveiCoin;
    public Text titleText;
    public Button receverCoin;
    private DailyMissionGoal dailyMissionGoal;
    
    public static event Action<int> OnCompletedMission;
    private void Start() {
        titleText.text = title;
        receverCoin.interactable = false;
        if(completed && !isReceveiCoin){
            receverCoin.interactable = true;
        }else{
            receverCoin.interactable = false;
        }

        // if(isReceveiCoin){
        // }
    }

    public void HandleCompletedMission(){
        if(completed){
            receverCoin.interactable = false;
            dailyMissionGoal.isReceveiCoin = true;
            dailyMissionGoal.title = "Đã hoàn thành";
            titleText.text = "Đã hoàn thành";
            OnCompletedMission?.Invoke(goldReward);
        }
    }

    public void SetData(DailyMissionGoal dailyMissionGoal) {
        index = dailyMissionGoal.index;
        completed = dailyMissionGoal.completed;
        title = dailyMissionGoal.title;
        description = dailyMissionGoal.description;
        goldReward = dailyMissionGoal.goldReward;
        isReceveiCoin = dailyMissionGoal.isReceveiCoin;
        //tao mot daily gan bang daily truyen vao
        this.dailyMissionGoal = dailyMissionGoal;
    }
}
