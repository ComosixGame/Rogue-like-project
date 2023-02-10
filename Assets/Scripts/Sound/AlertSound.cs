using UnityEngine;

public class AlertSound : MonoBehaviour
{
    private SettingData settingData;
    private GameManager gameManager;
    private SoundManager soundManager;
    private AudioSource audioSource;

    private void Awake() {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        audioSource.mute = gameManager.settingData.mute;
    }

    private void OnEnable() {
        soundManager.OnMute += muteGame;
    }

    private void OnDisable() {
        soundManager.OnMute -= muteGame;
    }

    private void muteGame(bool mute){
        audioSource.mute = mute;
    }
}
