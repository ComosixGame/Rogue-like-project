using System;
using System.Collections;
using UnityEngine;
using MyCustomAttribute;

public class EnergyManager : Singleton<EnergyManager>
{
    [SerializeField] private int maxEnergy;
    [SerializeField, Label("Time to recover energy(s)")] private float timeToRecoverEnergy;
    [SerializeField] private int recoverEnergy;
    [SerializeField] private int energy;
    private DateTime energyUpdateDateTime;
    private float timePassed = 0;
    [SerializeField, ReadOnly] private float timeLeft = 0;
    public event Action<int> OnRecoverEnergy;
    public event Action<float> OnEnergyRecoverTimerCounter;
    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
    }

    private void Start() {
        StartCoroutine(EnergyRecoverCoroutine());
        LoadData();
    }

    private void OnApplicationQuit() {
        gameManager.SaveEnergy(energy, energyUpdateDateTime);
    }   

    public void SpendEnergy(int energy) {
        bool isEnough = energy <= this.energy;
        if(isEnough) {
            this.energy -= energy;
            energyUpdateDateTime = DateTime.Now;
        }
    }

    private void RecoverEnergy() {
        if(energy >= maxEnergy) return;
        timePassed = (float)(DateTime.Now - energyUpdateDateTime).TotalSeconds;

        if(timePassed >= timeToRecoverEnergy) {
            int recoveredEnergy = energy + recoverEnergy;
            energy = recoveredEnergy < maxEnergy ? recoveredEnergy : maxEnergy;
            OnRecoverEnergy?.Invoke(energy);
            energyUpdateDateTime = DateTime.Now;
        }

        timeLeft = timeToRecoverEnergy - timePassed;
    }

    private void LoadData() {
        if(gameManager.firstTimeStart) {
            energy = maxEnergy;
        } else {
            energy = gameManager.energy;
            energyUpdateDateTime = gameManager.energyUpdateDateTime;
            RecoverEnergyOnGameload();
        }
    }

    private void RecoverEnergyOnGameload() {
        timePassed = (float)(DateTime.Now - energyUpdateDateTime).TotalSeconds;
        
        if(timePassed >= timeToRecoverEnergy) {
            int timesRecovered = Mathf.FloorToInt(timePassed/timeToRecoverEnergy);
            int recoveredEnergy = recoverEnergy * timesRecovered;
            energy = recoveredEnergy < maxEnergy ? recoveredEnergy : maxEnergy;
            energyUpdateDateTime = DateTime.Now.AddSeconds(timesRecovered * timeToRecoverEnergy - timePassed);
            timeLeft = timesRecovered * timeToRecoverEnergy - timePassed;
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
