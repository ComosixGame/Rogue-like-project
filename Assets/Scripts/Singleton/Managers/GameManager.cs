using System;
using System.Collections.Generic;
using UnityEngine;
using MyCustomAttribute;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    /**
     * Sử dụng Player để tham chiếu đển nhân vật của player
     * Event OnSelectedPlayer để lắng nghe hành động chọn player dữ liệu đươc gửi đi là Transform player
     * enemiesCount trả về số lượng enemies còn lại
     * Event OnEnemiesDestroyed để lắng nghe khi số enemies bằng 0
     * Event OnUpdateCoin để lặng nghe khi coin đc update dữ liệu gửi đi là số lượng coin
     * OnPause, OnResume sự kiện pause và resume
     */

    [ReadOnly, SerializeField] private int amountCoins;
    [ReadOnly, SerializeField] private List<Transform> enemies = new List<Transform>();
    public int enemiesCount {
        get {
            return enemies.Count;
        }
    }
    [ReadOnly] public int levels, waves;
    public Transform player {get; private set;}
    public Transform cam {get; private set;}
    
    public event Action OnEnemiesDestroyed;
    public event Action<Transform> OnSelectedPlayer;
    public event Action OnPause;
    public event Action OnResume;
    public event Action<int> OnUpdateCoin;

    public event Action<int> OnUpdateCoinPlayer;
    public event Action OnupdateInfoCharacter;
    private PlayerData playerData;
    public int coinPlayer {
        get {
           return playerData.coin;
        }
    }

    public int characterSeleted {
        get {
            return playerData.characterSeleted;
        }
    }

    public List<int> characterOwn {
        get {
            return playerData.characters;
        }
    }

    override protected void Awake() {
        base.Awake();
        playerData = PlayerData.Load();
        Debug.Log(playerData.coin);
    }


    public void EndGame(bool isWin) {
        //Thuực hiện việc update coin khi lose game
        //Được gọi khi thua mỗi ware
        playerData.coin += amountCoins;
        playerData.Save();
    }

    //buy character
    public bool BuyItem(int indexItem, int priceCharacter){
        if(playerData.coin >= priceCharacter) {
            if(playerData.characters.IndexOf(indexItem) == -1) {
                playerData.characters.Add(indexItem);
                playerData.coin -= priceCharacter;
                PlayerData.Load();
                playerData.Save();
                OnUpdateCoinPlayer?.Invoke(playerData.coin);
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    //selected character
    public void SelectedCharacter(int indexCharacter){
        if(playerData.characters.IndexOf(indexCharacter) != -1) {
            playerData.characterSeleted = indexCharacter;
            playerData.Save();
            OnupdateInfoCharacter?.Invoke();
        }
    }

    public List<Transform> GetEnemies() {
        return enemies;
    }

    public void AddEnemy(Transform enemy) {
        enemies.Add(enemy);
    }

    //remove enemy khỏi danh sách enemies
    public void RemoveEnemy(Transform enemy) {
        enemies.Remove(enemy);
        if(enemiesCount == 0) {
            OnEnemiesDestroyed?.Invoke();
        }
    }

    //clear toàn bộ enemy
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

    //update tiền nhặt đươc;
    public void UpdateCoin(int amount) {
        amountCoins += amount;
        OnUpdateCoin?.Invoke(amountCoins);
    }

    //set tham chiếu player
    public void SelectPlayer(Transform player) {
        this.player = player;
        OnSelectedPlayer?.Invoke(this.player);
    }

    
}
