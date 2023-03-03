using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DailyMissionGoal
{
    public string title;
    public int index;
    public bool completed;
    public string description;
    public int experienceReward;
    public int goldReward;
    public GoalTypes goalType;
    public string nameQuestGoal;
    public int requiredAmount;
    public int currentAmount;
    public bool isReceveiCoin;
    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }

    public bool EnemyKilled()
    {
        if(goalType == GoalTypes.Kill)
            currentAmount++;
        return currentAmount >= requiredAmount;
    }

    public bool ItemCollected()
    {
        if(goalType == GoalTypes.Gathering)
            currentAmount++;
        return currentAmount >= requiredAmount;
    }

    public bool Starting()
    {
        if(goalType == GoalTypes.Starting)
        {
            currentAmount = 0;
        }
        return currentAmount >= requiredAmount;
    }

    public DailyMissionGoal DailyMissionGoalClone() {
        return this.MemberwiseClone() as DailyMissionGoal;
    }
}   

public enum GoalTypes
{
    Kill,
    Gathering,
    Starting
}
