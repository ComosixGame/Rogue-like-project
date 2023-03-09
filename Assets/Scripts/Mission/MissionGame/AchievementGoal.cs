using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AchievementGoal
{
    public string title;
    public int index;
    public bool completed;
    public string description;
    public int goldReward;
    public AchievementTypes goalType;
    public string nameQuestGoal;
    public int requiredAmount;
    public int currentAmount;
    public bool isReceveiCoin;

    public bool IsReached(){
        return (currentAmount >= requiredAmount);
    }

    public bool EnemyKilled(){
        if(goalType == AchievementTypes.Kill){
            currentAmount++;
        }

        return currentAmount >= requiredAmount;
    }

    public bool ItemCollected(){
        if(goalType == AchievementTypes.Gathering){
            currentAmount++;
        }

        return currentAmount >= requiredAmount;
    }

    public AchievementGoal AchievementGoalClone(){
        return this.MemberwiseClone() as AchievementGoal;
    }

}

public enum AchievementTypes
{
    Kill,
    Gathering,
}
