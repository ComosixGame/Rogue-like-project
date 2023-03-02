using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using MyCustomAttribute;
using UnityEngine.UI;

public class DailyMissions : MonoBehaviour
{
  [SerializeField] private DailyMission DailyMissionPrefab;
  [SerializeField, ReadOnly] public List<DailyMission> dailyMissions = new List<DailyMission>();
  [SerializeField] private Transform DailyMissionParent;
  private List<int> mshowed = new List<int>();
  private GameManager gameManager;
  private DailyMissionManager dailyMissionManager;

  private void Awake()
  {
    gameManager = GameManager.Instance;
    dailyMissionManager = DailyMissionManager.Instance;
  }

  private void OnEnable()
  {
    dailyMissionManager.OnRandomDailyMission += RandomDailyMission;
    dailyMissionManager.OnRenderDailyMission += RenderDailyMission;
  }

  public void RandomDailyMissionRender(){
    //Xu ly viec random sau khi het thoi gian
  }

  public void RandomDailyMission(List<DailyMissionGoal> dailyMissionGoals)
  {
    int loop = dailyMissionGoals.Count < 10 ? dailyMissionGoals.Count : 10;
    foreach (DailyMission mission in dailyMissions)
    {
      Destroy(mission.gameObject);
    }
    //reset danh sach
    mshowed.Clear();
    dailyMissions.Clear();
    dailyMissionManager.displayeDailyMissions.Clear();

    for (int i = 0; i < loop; i++)
    {
      int random = Random.Range(0, dailyMissionGoals.Count);
      while (mshowed.IndexOf(random) != -1)
      {
        random = Random.Range(0, dailyMissionGoals.Count);
      }
      mshowed.Add(random);
      DailyMission dailyMission = Instantiate(DailyMissionPrefab);
      dailyMissions.Add(dailyMission);
      dailyMission.transform.SetParent(DailyMissionParent, false);
      dailyMission.index = random;

      DailyMissionGoal dailyMissionGoal = dailyMissionGoals[random];

      dailyMission.SetData(dailyMissionGoal);
      dailyMissionManager.displayeDailyMissions.Add(dailyMissionGoal);
    }

    gameManager.SaveDailyMissionGoals(dailyMissionManager.displayeDailyMissions);
  }

  public void RenderDailyMission()
  {
    foreach (DailyMission mission in dailyMissions)
    {
      Destroy(mission.gameObject);
    }
    //reset danh sach
    mshowed.Clear();
    dailyMissions.Clear();
    dailyMissionManager.displayeDailyMissions.Clear();
    foreach (DailyMissionGoal dailyMissionGoal in gameManager.displayeDailyMissions)
    {
      DailyMission dailyMission = Instantiate(DailyMissionPrefab);
      dailyMission.transform.SetParent(DailyMissionParent, false);
      dailyMission.SetData(dailyMissionGoal);
      dailyMissions.Add(dailyMission);
      dailyMissionManager.displayeDailyMissions.Add(dailyMissionGoal);
    }
    
  }
  private void OnDisable()
  {
    dailyMissionManager.OnRandomDailyMission -= RandomDailyMission;
    dailyMissionManager.OnRenderDailyMission -= RenderDailyMission;
  }
}
