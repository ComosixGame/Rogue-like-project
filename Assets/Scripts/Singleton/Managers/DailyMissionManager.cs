using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyCustomAttribute;

public class DailyMissionManager : Singleton<DailyMissionManager>
{
  [SerializeField] private DailyMissionScriptable dailyMissionScriptable;
  private GameManager gameManager;
  private float timePassed = 0;
  [SerializeField, Label("Time to recover daily mission(s)")] private float timeToRecoverDaily;
  private DateTime dailyUpdateDateTime;
  public List<DailyMissionGoal> dailyMissions { get; set; }
  public List<DailyMissionGoal> displayeDailyMissions;
  public event Action<List<DailyMissionGoal>> OnRandomDailyMission;
  public event Action OnRenderDailyMission;
  private LoadSceneManager loadSceneManager;
  protected override void Awake()
  {
    base.Awake();
    gameManager = GameManager.Instance;
    loadSceneManager = LoadSceneManager.Instance;
    dailyMissions = new List<DailyMissionGoal>();
    displayeDailyMissions = new List<DailyMissionGoal>();

    foreach (DailyMissionGoal dailyMission in dailyMissionScriptable.dailyMissions)
    {
      dailyMissions.Add(dailyMission.DailyMissionGoalClone());
    }
  }

  private void OnEnable()
  {
    EnemyDamageble.OnEnemiesDestroy += KillEnemy;
    loadSceneManager.OnLoadScene += OnLoadScene;
    loadSceneManager.OnSceneLoaded += SceneLoaded;
  }

  private void OnDisable()
  {
    EnemyDamageble.OnEnemiesDestroy -= KillEnemy;
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
  }

  private void OnApplicationQuit()
  {
    gameManager.SaveTimeDailyMission(dailyUpdateDateTime);
    gameManager.SaveDailyMissionGoals(displayeDailyMissions);
  }



  public void LoadData()
  {
    if (gameManager.firstTimeStart)
    {
      OnRandomDailyMission?.Invoke(dailyMissions);
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

  private IEnumerator CountDownSecondTime()
  {
    while (true)
    {
      timePassed = (float)(DateTime.Now - dailyUpdateDateTime).TotalSeconds;
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


  public void CountDownSecondTimeOnLoadGame()
  {
    dailyUpdateDateTime = gameManager.dailyMissionDateTime;
    timePassed = (float)(DateTime.Now - dailyUpdateDateTime).TotalSeconds;

    if (timePassed >= timeToRecoverDaily)
    {
      // dailyMissions.Clear();
      // foreach (DailyMissionGoal dailyMission in dailyMissionScriptable.dailyMissions)
      // {
      //   dailyMissions.Add(dailyMission.DailyMissionGoalClone());
      // }
      gameManager.displayeDailyMissions.Clear();
      OnRandomDailyMission?.Invoke(dailyMissions);
      dailyUpdateDateTime = DateTime.Now;
    }
    else
    {
      //data cu
      OnRenderDailyMission?.Invoke();
    }

  }

  private void OnLoadScene()
  {
    gameManager.SaveDailyMissionGoals(displayeDailyMissions);

  }

  private void SceneLoaded(Scene scene)
  {
    OnRenderDailyMission?.Invoke();
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
      dailyMission.completed = dailyMission.ItemStarting();
    }
  }

  public void CollecteItem()
  {
    foreach (DailyMissionGoal dailyMission in displayeDailyMissions)
    {
      dailyMission.completed = dailyMission.ItemCollected();
    }
  }

}
