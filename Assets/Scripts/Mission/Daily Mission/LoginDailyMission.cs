using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyCustomAttribute;
using System;
using UnityEngine.UI;

public class LoginDailyMission : MonoBehaviour
{
    public int indexLoginGameMission = 0;
    public static event Action<int> OnLoginGame;
    // Start is called before the first frame update

    public void LoginMission(int indexLoginGameMission){
       OnLoginGame?.Invoke(indexLoginGameMission);
    }
}
