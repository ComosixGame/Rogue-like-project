using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyCustomAttribute;

public class DailyMissionManager : Singleton<DailyMissionManager>
{
  [SerializeField] private DailyMissionScriptable dailyMissionScriptable;
  [SerializeField] private AchievementScriptAble achievementScriptAble;
  private GameManager gameManager;
  private int timePassed = 0;
  [SerializeField, Label("Time to recover daily mission(s)")] private int timeToRecoverDaily;
  [SerializeField, ReadOnly] private int timeLeft = 0;
  private DateTime dailyUpdateDateTime;
  public List<DailyMissionGoal> dailyMissions { get; set; }
  public List<AchievementGoal> achievements { get; set; }
  public List<DailyMissionGoal> displayeDailyMissions;
  public List<AchievementGoal> displayAchievements;
  public event Action<List<DailyMissionGoal>> OnRandomDailyMission;
  public event Action OnRenderDailyMission;
  public event Action OnRenderAchievements;

  public event Action<List<AchievementGoal>> OnRandomAchievements;
  private LoadSceneManager loadSceneManager;
  protected override void Awake()
  {
    base.Awake();
    gameManager = GameManager.Instance;
    loadSceneManager = LoadSceneManager.Instance;
    dailyMissions = new List<DailyMissionGoal>();
    displayeDailyMissions = new List<DailyMissionGoal>();
    achievements = new List<AchievementGoal>();
    displayAchievements = new List<AchievementGoal>();

    foreach (DailyMissionGoal dailyMission in dailyMissionScriptable.dailyMissions)
    {
      dailyMissions.Add(dailyMission.DailyMissionGoalClone());
    }

    foreach (AchievementGoal achievement in achievementScriptAble.achievementGoals)
    {
      achievements.Add(achievement.AchievementGoalClone());
    }
  }

  private void OnEnable()
  {
    EnemyDamageble.OnEnemiesDestroy += KillEnemy;
    loadSceneManager.OnLoadScene += OnLoadScene;
    loadSceneManager.OnSceneLoaded += SceneLoaded;
    UIManager.OnReloadDailyMission += CountDownSecondTimeOnLoadGame;
    EnemyDamageble.OnEnemiesDestroy += AchievementKillEnemy;
  }

  private void OnDisable()
  {
    EnemyDamageble.OnEnemiesDestroy -= KillEnemy;
    EnemyDamageble.OnEnemiesDestroy -= AchievementKillEnemy;
    UIManager.OnReloadDailyMission -= CountDownSecondTimeOnLoadGame;
    if (loadSceneManager != null)
    {
      loadSceneManager.OnLoadScene -= OnLoadScene;
      loadSceneManager.OnSceneLoaded -= SceneLoaded;
    }
  }

  private void Start()
  {
    CountDownSecondTimeOnLoadGame();
    StartCoroutine(CountDownSecondTime());
    LoadData();
    TimeRemainingDailyMission();
    //Thực hiện check khi vào game nhiem vu
    //Check nhiem vu da thanh cong
    StartGame();
    //Khi bam nút mission thì lưu lại thông tin vào file save
    gameManager.SaveDailyMissionGoals(displayeDailyMissions);
    gameManager.saveAchievements(displayAchievements);
  }
  

  private void OnApplicationQuit()
  {
    gameManager.SaveTimeDailyMission(dailyUpdateDateTime);
    gameManager.SaveDailyMissionGoals(displayeDailyMissions);
    gameManager.saveAchievements(displayAchievements);
  }

  public void LoadData()
  {
    if (gameManager.firstTimeStart)
    {
      OnRandomDailyMission?.Invoke(dailyMissions);
      OnRandomAchievements?.Invoke(achievements);
    }
  }

  public void RandomDailyMission()
  {
    gameManager.PlayerDataSave();
  }

  public void RenderDailyMission()
  {
    gameManager.PlayerDataSave();
  }

  //Ham tinh thoi gian va ramdom ra nhiem vu
  private IEnumerator CountDownSecondTime()
  {
    while (true)
    {
      timePassed = (int)(DateTime.Now - dailyUpdateDateTime).TotalSeconds;
      if (timePassed >= timeToRecoverDaily)
      {
        dailyMissions.Clear();
        foreach (DailyMissionGoal dailyMission in dailyMissionScriptable.dailyMissions)
        {
          dailyMissions.Add(dailyMission.DailyMissionGoalClone());
        }
        gameManager.displayeDailyMissions.Clear();
        OnRandomDailyMission?.Invoke(dailyMissions);
        dailyUpdateDateTime = DateTime.Now;
      }
      yield return new WaitForSeconds(1f);
    }
  }


  //Dem nguoc thoi gian va va render ra nhiem vu tuy thuoc dieu kien
  public void CountDownSecondTimeOnLoadGame()
  {
    dailyUpdateDateTime = gameManager.dailyMissionDateTime;
    timePassed = (int)(DateTime.Now - dailyUpdateDateTime).TotalSeconds;

    if (timePassed >= timeToRecoverDaily)
    {
      gameManager.displayeDailyMissions.Clear();
      OnRandomDailyMission?.Invoke(dailyMissions);
      dailyUpdateDateTime = DateTime.Now;
    }
    else
    {
      //data cu
      timeLeft = timeToRecoverDaily - timePassed;
      OnRenderDailyMission?.Invoke();
    }

  }

  //Function caculate time remaining of daily mission
  public void TimeRemainingDailyMission(){
    timeLeft = timeToRecoverDaily - timePassed;
    float hour, minute;
    hour = timeLeft / 3600;
    minute = timeLeft % 3600 / 60;
    //Debug.Log(hour + " : " + minute);
  }


  private void OnLoadScene()
  {
    gameManager.SaveDailyMissionGoals(displayeDailyMissions);
    gameManager.saveAchievements(displayAchievements);
  }

  private void SceneLoaded(Scene scene)
  {
    OnRenderDailyMission?.Invoke();
    OnRenderAchievements?.Invoke();
  }

  private void KillEnemy(Vector3 dir)
  {
    foreach (DailyMissionGoal dailyMission in displayeDailyMissions)
    {
      dailyMission.completed = dailyMission.EnemyKilled();
    }
  }

  public void StartGame()
  {
    foreach (DailyMissionGoal dailyMission in displayeDailyMissions)
    {
      dailyMission.completed = dailyMission.Starting();
    }
  }

  public void CollecteItem()
  {
    foreach (DailyMissionGoal dailyMission in displayeDailyMissions)
    {
      dailyMission.completed = dailyMission.ItemCollected();
    }
  }


  private void AchievementKillEnemy(Vector3 dir){
    foreach(AchievementGoal achievement in displayAchievements){
      achievement.completed = achievement.EnemyKilled();
    }
  }

  public void AchievementCollecteItem(){
    foreach (AchievementGoal achievement in displayAchievements)
    {
      achievement.completed = achievement.EnemyKilled();
    }
  }

}
