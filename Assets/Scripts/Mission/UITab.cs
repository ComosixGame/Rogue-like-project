using UnityEngine;
using UnityEngine.UI;

public class UITab : MonoBehaviour
{
    [SerializeField] private Button DailyMissionBtn;
    [SerializeField] private Button AchievementBtn;
    [SerializeField] private GameObject DailyMissionUI;
    [SerializeField] private GameObject AchievementUI;

    private void Start() {
        HandleDailyMissionBtn();
    }

    public void HandleDailyMissionBtn(){
        DailyMissionUI.SetActive(true);
        AchievementUI.SetActive(false);
    }

    public void HandleAchievementBtn(){
        DailyMissionUI.SetActive(false);
        AchievementUI.SetActive(true);
    }
}
