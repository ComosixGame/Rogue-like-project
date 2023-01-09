using System;
using System.Collections.Generic;
using UnityEngine;
using MyCustomAttribute;

public class GameManager : Singleton<GameManager>
{
    [ReadOnly, SerializeField] private int amountCoins;
    [ReadOnly, SerializeField] private List<Transform> enemies = new List<Transform>();
    public int enemiesCount {
        get {
            return enemies.Count;
        }
    }
    [ReadOnly] public int levels, waves;
    public Transform player {get; private set;}
    public event Action OnEnemiesDestroyed;
    public event Action<Transform> OnSelectedPlayer;
    public event Action OnPause;
    public event Action OnResume;
    public event Action<int> OnUpdateCoin;

    public List<Transform> GetEnemies() {
        return enemies;
    }

    public void AddEnemy(Transform enemy) {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(Transform enemy) {
        enemies.Remove(enemy);
        if(enemiesCount == 0) {
            OnEnemiesDestroyed?.Invoke();
        }
    }

    public void ClearEnemies() {
        enemies.Clear();
    }
    
    public void PauseGame() {
        Time.timeScale = 0;
        OnPause?.Invoke();
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        OnResume?.Invoke();
    }

    public void UpdateCoin(int amount) {
        amountCoins += amount;
        OnUpdateCoin?.Invoke(amountCoins);
    }

    public void SelectPlayer(Transform player) {
        this.player = player;
        OnSelectedPlayer?.Invoke(this.player);
    }
    
}
