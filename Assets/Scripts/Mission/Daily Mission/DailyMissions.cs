using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using MyCustomAttribute;
using UnityEngine.UI;

public class DailyMissions : MonoBehaviour
{
  [SerializeField] private DailyMissionCard DailyMissionPrefab;
  [SerializeField] private Transform DailyMissionParent;
  [SerializeField, ReadOnly] public List<DailyMissionCard> dailyMissions = new List<DailyMissionCard>();
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

  //Random ra nhiem vu
  public void RandomDailyMission(List<DailyMissionGoal> dailyMissionGoals)
  {
    int loop = dailyMissionGoals.Count < 10 ? dailyMissionGoals.Count : 10;
    foreach (DailyMissionCard mission in dailyMissions)
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
      DailyMissionCard dailyMission = Instantiate(DailyMissionPrefab);
      dailyMissions.Add(dailyMission);
      dailyMission.transform.SetParent(DailyMissionParent, false);
      dailyMission.index = random;
      DailyMissionGoal dailyMissionGoal = dailyMissionGoals[random];
      dailyMission.SetData(dailyMissionGoal);
      dailyMissionManager.displayeDailyMissions.Add(dailyMissionGoal);
    }

    gameManager.SaveDailyMissionGoals(dailyMissionManager.displayeDailyMissions);
  }

  //thuc hien render lai nhiem vu cu
  public void RenderDailyMission()
  {
    foreach (DailyMissionCard mission in dailyMissions)
    {
      Destroy(mission.gameObject);
    }
    //reset danh sach
    mshowed.Clear();
    dailyMissions.Clear();
    dailyMissionManager.displayeDailyMissions.Clear();
    foreach (DailyMissionGoal dailyMissionGoal in gameManager.displayeDailyMissions)
    {
      DailyMissionCard dailyMission = Instantiate(DailyMissionPrefab);
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
