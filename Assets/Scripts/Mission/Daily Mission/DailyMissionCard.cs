using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DailyMissionCard : MonoBehaviour
{
    public int index;
    public bool completed;
    public string title;
    public string description;
    public int experienceReward;
    public int goldReward;
    public bool isReceveiCoin;
    public TMP_Text titleText;
    public TMP_Text goldRewardText;
    public Button receverCoin;
    private DailyMissionGoal dailyMissionGoal;
    
    public static event Action<int> OnCompletedMission;
    private void Start() {
        titleText.text = title;
        goldRewardText.text = goldReward.ToString();
        receverCoin.interactable = false;
        if(completed && !isReceveiCoin){
            receverCoin.interactable = true;
            goldRewardText.text = "Claim";
        }else{
            receverCoin.interactable = false;
        }
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
