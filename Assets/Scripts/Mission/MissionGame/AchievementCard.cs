using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementCard : MonoBehaviour
{
    public int index;
    public bool completed;
    public string title;
    public string description;
    public int goldReward;
    public bool isReceveiCoin;
    public Button receverCoin;
    public TMP_Text titleText;
    public TMP_Text goldRewardText;
    private AchievementGoal achievementGoal;

    //Singleton
    private SoundManager soundManager;

    //Action
    public static event Action<int> OnCompletedAchievement;

    //sound
    public AudioClip btnSound;

    private void Awake() {
        soundManager = SoundManager.Instance;
    }

    private void Start() {
        titleText.text = title;
        goldRewardText.text = goldReward.ToString();
        if(completed && !isReceveiCoin){
            receverCoin.interactable = true;
            goldRewardText.text = "Claim";
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

    public void PlayBtnSound(){
        soundManager.PlaySound(btnSound);
    }
}
