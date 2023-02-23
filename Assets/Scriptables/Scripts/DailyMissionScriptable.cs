using UnityEngine;

[CreateAssetMenu(fileName = "Daily Mission Manager", menuName = "Scriptable Manager/Daily Mission Manager")]
public class DailyMissionScriptable : ScriptableObject
{
    [System.Serializable]
    public class DailyMission{
        public string nameDailyMission;
        public int coinReceive;
    }

    public DailyMission[] dailyMissions;
}
