using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISettings : MonoBehaviour
{
    [SerializeField] private Toggle fps30, fps60, mute;
    [SerializeField] private TextMeshProUGUI warningText;

    private GameManager gameManager;
    private PlayerData playerData;
    private SoundManager soundManager;

    private void Awake() {
        gameManager = GameManager.Instance;
        playerData = PlayerData.Load();
        soundManager = SoundManager.Instance;
    }
    private void OnEnable() {
        //resSliderScale.onValueChanged.AddListener(SetScaleRes);
        fps30.onValueChanged.AddListener(OnValueChangedFps30);
        fps60.onValueChanged.AddListener(OnValueChangedFps60);
        mute.onValueChanged.AddListener(MuteGame);
    }

    private void Start() {
        //resSliderScale.value = gameManager.settingData.resolutionScale * 10;
        float fps = gameManager.settingData.fps;
        fps30.isOn = fps == 30;
        fps60.isOn = fps == 60;
        mute.isOn  = gameManager.settingData.mute;

        warningText.gameObject.SetActive(false);
    }

    private void OnDisable() {
        //resSliderScale.onValueChanged.RemoveListener(SetScaleRes);
        fps30.onValueChanged.RemoveListener(OnValueChangedFps30);
        fps60.onValueChanged.RemoveListener(OnValueChangedFps60);
        mute.onValueChanged.RemoveListener(MuteGame);
    }

    // public void SetScaleRes(float value){
    //     float scale = value/10;
    //     if(gameManager.settingData.resolutionScale != scale) {
    //         warningText.gameObject.SetActive(true);
    //         gameManager.settingData.resolutionScale = scale;
    //         gameManager.settingData.Save();
    //     }
    //     resSliderText.text = "Resolution scale:" + scale.ToString();
    // }

     private void OnValueChangedFps30(bool check) {
        if(check) {
            if(gameManager.settingData.fps != 30) {
                warningText.gameObject.SetActive(true);
                gameManager.settingData.fps = 30;
                fps60.isOn = false;
                gameManager.settingData.Save();
            }
        }
    }

    private void OnValueChangedFps60(bool check) {
        if(check) {
            if(gameManager.settingData.fps != 60) {
                warningText.gameObject.SetActive(true);
                gameManager.settingData.fps = 60;
                fps30.isOn = false;
                gameManager.settingData.Save();
            }
        }
    }

    public void MuteGame(bool mute){
        soundManager.MuteGame(mute);
    }

}
