using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMission : MonoBehaviour
{
    public GameObject _dailyMisson;
    public GameObject _achievements;

    private void Start() {
        _dailyMisson.SetActive(true);
        _achievements.SetActive(false);
    }

    public void BtnDailyMisson(){
        _dailyMisson.SetActive(true);
        _achievements.SetActive(false);
    }
    public void BtnAchievements(){
        _dailyMisson.SetActive(false);
        _achievements.SetActive(true);
    }
}
