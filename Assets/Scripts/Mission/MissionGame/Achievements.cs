using System.Collections.Generic;
using UnityEngine;
using MyCustomAttribute;

public class Achievements : MonoBehaviour
{
    [SerializeField] private AchievementScriptAble achievementScriptAble;
    [SerializeField] private AchievementCard achievementCardPrefab;
    [SerializeField] private Transform achievementParent;
    [SerializeField, ReadOnly] public List<AchievementCard> achievements = new List<AchievementCard>();
    private GameManager gameManager;
    private DailyMissionManager dailyMissionManager;
    private List<int> mshowed = new List<int>();

    private void Awake() {
        gameManager = GameManager.Instance;
        dailyMissionManager = DailyMissionManager.Instance;
    }

    private void OnEnable() {
        dailyMissionManager.OnRandomAchievements += RandomAchievements;
        dailyMissionManager.OnRenderAchievements += RenderAchievements;
    }

    //Thực hiện render ra thành tích khởi tạo từ scriptable
    public void RandomAchievements(List<AchievementGoal> achievementGoals){
        achievements.Clear();
        dailyMissionManager.displayAchievements.Clear();
        foreach (AchievementGoal achievement in achievementGoals)
        {
            AchievementCard achievementEle = Instantiate(achievementCardPrefab);
            achievementEle.transform.SetParent(achievementParent, false);
            achievementEle.SetData(achievement);
            dailyMissionManager.displayAchievements.Add(achievement);
        }

        gameManager.saveAchievements(dailyMissionManager.achievements);
    }

    //thực hiện render ra thành tích lưu trong máy
    public void RenderAchievements(){
        mshowed.Clear();
        achievements.Clear();
        dailyMissionManager.displayAchievements.Clear();
        foreach (AchievementGoal achievement in gameManager.displayAchievement)
        {
            AchievementCard achievementEle = Instantiate(achievementCardPrefab);
            achievementEle.transform.SetParent(achievementParent, false);
            achievementEle.SetData(achievement);
            achievements.Add(achievementEle);
            dailyMissionManager.displayAchievements.Add(achievement);
        }
    }

    private void OnDisable(){
        dailyMissionManager.OnRandomAchievements -= RandomAchievements;
        dailyMissionManager.OnRenderAchievements -= RenderAchievements;
    }
}
