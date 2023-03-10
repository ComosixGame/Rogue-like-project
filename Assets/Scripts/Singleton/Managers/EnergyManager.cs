using System;
using System.Collections;
using UnityEngine;
using MyCustomAttribute;
using UnityEngine.SceneManagement;

public class EnergyManager : Singleton<EnergyManager>
{
    [SerializeField] private int _maxEnergy;
    public float maxEnergy {get {
        return _maxEnergy;
    }}
    [SerializeField, Label("Time to recover energy(s)")] private float timeToRecoverEnergy;
    [SerializeField] private int recoverEnergy;
    [SerializeField] private int energy;
    private DateTime energyUpdateDateTime;
    private float timePassed = 0;
    [SerializeField, ReadOnly] private float timeLeft = 0;
    public event Action<int> OnUpdateEnergy;
    public event Action<float> OnEnergyRecoverTimerCounter;
    public event Action OnPlayChapter;
    public event Action OnNotEnoughEnergy;
    private GameManager gameManager;
    private LoadSceneManager loadSceneManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
        loadSceneManager = LoadSceneManager.Instance;
        LoadData();
    }

    private void OnEnable() {
        if(loadSceneManager != null){
            loadSceneManager.OnSceneLoaded += SceneLoaded;
        }
    }

    private void OnDisable() {
        if(loadSceneManager != null){
            loadSceneManager.OnSceneLoaded -= SceneLoaded;
        }
    }

    private void Start() {
        StartCoroutine(EnergyRecoverCoroutine());
    }

    private void OnApplicationQuit() {
        gameManager.SaveEnergy(energy, energyUpdateDateTime);
    }   

    public void SpendEnergy(int energy) {
        bool isEnough = energy <= this.energy;
        if(isEnough) {
            this.energy -= energy;
            energyUpdateDateTime = DateTime.Now;
            OnUpdateEnergy?.Invoke(this.energy);
            OnPlayChapter?.Invoke();
        }
        else{
            OnNotEnoughEnergy?.Invoke();
        }
    }

    private void RecoverEnergy() {
        if(energy >= maxEnergy) return;
        timePassed = (float)(DateTime.Now - energyUpdateDateTime).TotalSeconds;

        if(timePassed >= timeToRecoverEnergy) {
            int recoveredEnergy = energy + recoverEnergy;
            energy = recoveredEnergy < _maxEnergy ? recoveredEnergy : _maxEnergy;
            energyUpdateDateTime = DateTime.Now;
            OnUpdateEnergy?.Invoke(energy);
        }

        timeLeft = timeToRecoverEnergy - timePassed;
    }

    private void LoadData() {
        if(gameManager.firstTimeStart) {
            energy = _maxEnergy;
        } else {
            energy = gameManager.energy;
            energyUpdateDateTime = gameManager.energyUpdateDateTime;
            RecoverEnergyOnGameload();
        }
        OnUpdateEnergy?.Invoke(energy);
    }

    private void SceneLoaded(Scene scene){
        OnUpdateEnergy?.Invoke(energy);
    }

    private void RecoverEnergyOnGameload() {
        timePassed = (float)(DateTime.Now - energyUpdateDateTime).TotalSeconds;
        
        if(timePassed >= timeToRecoverEnergy) {
            int timesRecovered = Mathf.FloorToInt(timePassed/timeToRecoverEnergy);
            int recoveredEnergy = recoverEnergy * timesRecovered;
            energy = recoveredEnergy < _maxEnergy ? recoveredEnergy : _maxEnergy;
            energyUpdateDateTime = DateTime.Now.AddSeconds(timesRecovered * timeToRecoverEnergy - timePassed);
            timeLeft = timesRecovered * timeToRecoverEnergy - timePassed;
            OnUpdateEnergy?.Invoke(energy);
        } else {
            timeLeft =  timeToRecoverEnergy -  timePassed;
        }
    }

    IEnumerator EnergyRecoverCoroutine() {
        while(true) {
            RecoverEnergy();
            yield return new WaitForSeconds(1f);
            if(energy < maxEnergy) {
                OnEnergyRecoverTimerCounter?.Invoke(timeLeft);
            }
        }
    }
}
