using System;
using UnityEngine;
using UnityEngine.UI;

public class AchievementCard : MonoBehaviour
{
    public int index;
    public bool completed;
    public string title;
    public string description;
    public int goldReward;
    public bool isReceveiCoin;
    public Button receverCoin;
    public Text titleText;
    private AchievementGoal achievementGoal;
    public static event Action<int> OnCompletedAchievement;
    private void Start() {
        titleText.text = title;
        if(completed && !isReceveiCoin){
            receverCoin.interactable = true;
        }else{
            receverCoin.interactable = false;
        }
    }

    public void HandleCompletedAchievement(){
        if(completed){
            receverCoin.interactable = false;
            achievementGoal.isReceveiCoin = true;
            achievementGoal.title = "Đã hoàn thành";
            titleText.text = "Đã hoàn thành";
            OnCompletedAchievement?.Invoke(goldReward);
        }
    }

    public void SetData(AchievementGoal achievementGoal){
        index = achievementGoal.index;
        completed = achievementGoal.completed;
        title = achievementGoal.title;
        description = achievementGoal.description;
        goldReward = achievementGoal.goldReward;
        isReceveiCoin = achievementGoal.isReceveiCoin;
        this.achievementGoal = achievementGoal;
    }
}
